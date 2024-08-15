using GameplayTags.Runtime.Attribute;
using System.Reflection.Emit;

namespace GameplayTags.Runtime.GameplayEffect;
#pragma warning disable SA1600

public enum StackPolicy : byte
{
	AggregateBySource,
	AggregateByTarget,
}

public enum StackLevelPolicy : byte
{
	AggregateLevels,
	SegregateLevels,
}

public enum StackLevelOverridePolicy : byte
{
	AlwaysKeep,
	AlwaysOverride,
	KeepHighest,
	KeepLowest,
}

public enum StackApplicationRefreshPolicy : byte
{
	RefreshOnSuccessfulApplication,
	NeverRefresh,
}

public enum StackRemovalPolicy : byte
{
	ClearEntireStack,
	RemoveSingleStackAndRefreshDuration,
}

public enum StackApplicationResetPeriodPolicy : byte
{
	ResetOnSuccessfulApplication,
	NeverReset,
}

public struct StackingData
{
	public int StackLimit; // All stackable effects
	public StackPolicy StackPolicy; // All stackable effects
	public StackLevelPolicy StackLevelPolicy; // All stackable effects
	public StackRemovalPolicy StackRemovalPolicy; // Aff stackable effects, infinite effects removal will count as expiration
	public StackApplicationRefreshPolicy? StackApplicationRefreshPolicy; // Effects with duration
	public StackLevelOverridePolicy? StackLevelOverridePolicy; // Effects with LevelStacking == AggregateLevels
	public StackApplicationResetPeriodPolicy? StackApplicationResetPeriodPolicy; // Periodic effects
}

public enum ModifierOperation : byte
{
	Add,
	Multiply,
	Divide,
	Override,
}

public struct Modifier
{
	public TagName Attribute;
	public ModifierOperation Operation;
	public ScalableInt Value;
}

public enum DurationType : byte
{
	Instant,
	Infinite,
	HasDuration,
}

public struct DurationData
{
	public DurationType Type;
	public float Duration;
}

public struct PeriodicData
{
	public float Period;
	public bool ExecuteOnApplication;
}

public class GameplayEffectData
{
	public List<Modifier> Modifiers { get; } = new ();
	//public List<Executions> Executions { get; } = new ();

	public string Name { get; }

	public DurationData DurationData { get; }

	public StackingData? StackingData { get; }

	public PeriodicData? PeriodicData { get; }

	public GameplayEffectData(
		string name,
		DurationData durationData,
		StackingData? stackingData,
		PeriodicData? periodicData)
	{
		Name = name;
		DurationData = durationData;
		StackingData = stackingData;
		PeriodicData = periodicData;

		// Should I really throw? Or just ignore (force null) the periodic data?
		if (periodicData.HasValue && durationData.Type == DurationType.Instant)
		{
			throw new Exception("Periodic effects can't be set as instant.");
		}

		// Should I really throw? Or just ignore (force null) the periodic data?
		if (durationData.Type != DurationType.HasDuration && durationData.Duration != 0)
		{
			throw new Exception($"Can't set duration if {nameof(DurationType)} is set to {durationData.Type}.");
		}

		if (stackingData.HasValue)
		{
			if (durationData.Type == DurationType.Instant)
			{
				throw new Exception($"{DurationType.Instant} effects can't have stacks.");
			}

			if (stackingData.Value.StackApplicationResetPeriodPolicy.HasValue != PeriodicData.HasValue)
			{
				throw new Exception($"Both {nameof(PeriodicData)} and {nameof(StackApplicationResetPeriodPolicy)} " +
					$"must be either defined or undefined.");
			}

			if (stackingData.Value.StackLevelPolicy == StackLevelPolicy.AggregateLevels !=
				stackingData.Value.StackLevelOverridePolicy.HasValue)
			{
				throw new Exception($"If {nameof(StackLevelPolicy)} is set {StackLevelPolicy.AggregateLevels}, " +
					$"{nameof(StackLevelOverridePolicy)} must be defined. And not defined if otherwise.");
			}

			if (durationData.Type == DurationType.HasDuration !=
				stackingData.Value.StackApplicationRefreshPolicy.HasValue)
			{
				throw new Exception($"Effects set as {DurationType.HasDuration} must define " +
					$" {nameof(StackApplicationRefreshPolicy)} and not define it if otherwise.");
			}
		}
	}
}

public struct GameplayEffectContext
{
	public object Instigator;
	public object EffectCauser;
}

public class GameplayEffect
{
	public GameplayEffectData EffectData { get; }

	public float Level { get; }

	public GameplayEffectContext Context { get; }

	public GameplayEffect(GameplayEffectData effectData, float level, GameplayEffectContext context)
	{
		EffectData = effectData;
		Level = level;
		Context = context;
	}

	public void Execute(AttributeSet targetAttributeSet)
	{
		foreach (var modifier in EffectData.Modifiers)
		{
			targetAttributeSet.AttributesMap[modifier.Attribute].AddToBaseValue(modifier.Value.GetValue(Level));
		}
	}
}

public class ActiveGameplayEffect
{
	private readonly AttributeSet _targetAttributeSet;

	private float _internalTime;

	public GameplayEffect GameplayEffect { get; }

	public float RemainingDuration { get; private set; }

	public float NextPeriodicTick { get; private set; }

	public float ExecutionCount { get; private set; }

	public bool IsExpired => GameplayEffect.EffectData.DurationData.Type == DurationType.HasDuration &&
		RemainingDuration <= 0;

	public ActiveGameplayEffect(GameplayEffect gameplayEffect, AttributeSet targetAttributeSet)
	{
		GameplayEffect = gameplayEffect;
		_targetAttributeSet = targetAttributeSet;
	}

	public void Apply()
	{
		_internalTime = 0;
		ExecutionCount = 0;
		RemainingDuration = GameplayEffect.EffectData.DurationData.Duration;

		foreach (var modifier in GameplayEffect.EffectData.Modifiers)
		{
			if (GameplayEffect.EffectData.PeriodicData.HasValue)
			{
				if (GameplayEffect.EffectData.PeriodicData.Value.ExecuteOnApplication)
				{
					GameplayEffect.Execute(_targetAttributeSet);
					ExecutionCount++;
				}

				NextPeriodicTick = GameplayEffect.EffectData.PeriodicData.Value.Period;
			}
			else
			{
				_targetAttributeSet.AttributesMap[modifier.Attribute].ApplyModifier(modifier.Value.GetValue(GameplayEffect.Level));
			}
		}
	}

	public void Unapply()
	{
		if (!GameplayEffect.EffectData.PeriodicData.HasValue)
		{
			foreach (var modifier in GameplayEffect.EffectData.Modifiers)
			{
				_targetAttributeSet.AttributesMap[modifier.Attribute].ApplyModifier(-modifier.Value.GetValue(GameplayEffect.Level));
			}
		}
	}

	public void Update(float deltaTime)
	{
		_internalTime += deltaTime;

		if (GameplayEffect.EffectData.PeriodicData.HasValue && _internalTime >= NextPeriodicTick)
		{
			GameplayEffect.Execute(_targetAttributeSet);
			ExecutionCount++;

			NextPeriodicTick += GameplayEffect.EffectData.PeriodicData.Value.Period;
		}

		if (GameplayEffect.EffectData.DurationData.Type == DurationType.HasDuration)
		{
			RemainingDuration -= deltaTime;
		}

		if (IsExpired)
		{
			Unapply();
		}
	}
}

public class GameplayEffectsManager
{
	private readonly List<ActiveGameplayEffect> _activeEffects = new ();

	// Could be a list of attributeSets;
	private AttributeSet _attributeSet;

	public GameplayEffectsManager(AttributeSet ownerAttributeSet)
	{
		_attributeSet = ownerAttributeSet;
	}

	public void ApplyEffect(GameplayEffect gameplayEffect)
	{
		if (gameplayEffect.EffectData.DurationData.Type != DurationType.Instant)
		{
			var activeEffect = new ActiveGameplayEffect(gameplayEffect, _attributeSet);
			_activeEffects.Add(activeEffect);
			activeEffect.Apply();
		}
		else
		{
			// This path is called "Execute" and should work for instant effects
			gameplayEffect.Execute(_attributeSet);
		}
	}

	public void UnapplyEffect(GameplayEffect gameplayEffect)
	{
		//_activeEffects.Remove(gameplayEffect);
	}

	// What if the game is a turn based game?
	public void UpdateEffects(float deltaTime)
	{
		foreach (var effect in _activeEffects)
		{
			effect.Update(deltaTime);
		}

		_activeEffects.RemoveAll(e => e.IsExpired);
	}
}
