using GameplayTags.Runtime.Attribute;

namespace GameplayTags.Runtime.GameplayEffect;

public class GameplayEffectsManager
{
	private readonly List<ActiveGameplayEffect> _activeEffects = new ();

	private IForgeEntity _owner;

	public GameplayEffectsManager(IForgeEntity owner)
	{
		_owner = owner;
	}

	// Should I allow a custom stackCount when applying?
	//public void ApplyEffect(GameplayEffect gameplayEffect, int stackCount = 1)
	public void ApplyEffect(GameplayEffect gameplayEffect)
	{
		//var effectEvaluatedData = new GameplayEffectEvaluatedData(gameplayEffect, _owner);

		if (gameplayEffect.EffectData.DurationData.Type != DurationType.Instant)
		{
			if (gameplayEffect.EffectData.StackingData.HasValue)
			{
				ActiveGameplayEffect? stackableEffect = null;

				stackableEffect = _activeEffects.Find(
					x => x.GameplayEffectEvaluatedData.GameplayEffect.EffectData == gameplayEffect.EffectData &&
						(gameplayEffect.EffectData.StackingData.Value.StackPolicy == StackPolicy.AggregateByTarget ||
						x.GameplayEffectEvaluatedData.GameplayEffect.Context.Instigator == gameplayEffect.Context.Instigator) &&
						(gameplayEffect.EffectData.StackingData.Value.StackLevelPolicy == StackLevelPolicy.AggregateLevels ||
						x.GameplayEffectEvaluatedData.GameplayEffect.Level == gameplayEffect.Level));

				if (stackableEffect is not null)
				{
					//var stackSucceeded = stackableEffect.AddStack(gameplayEffect, stackCount);
					var stackSucceeded = stackableEffect.AddStack(gameplayEffect);

					Console.WriteLine($"Stack application status: {stackSucceeded}");

					return;
				}
			}

			var activeEffect = new ActiveGameplayEffect(gameplayEffect, _owner);
			_activeEffects.Add(activeEffect);
			//activeEffect.Apply(stackCount);
			activeEffect.Apply();
		}
		else
		{
			// Execution shouldn't allow use of stackCount
			// This path is called "Execute" and should work for instant effects
			gameplayEffect.Execute(gameplayEffect, _owner);
		}
	}

	// For now, remove the first instance of the effect
	public void UnapplyEffect(GameplayEffect gameplayEffect)
	{
		ActiveGameplayEffect? effectToRemove = null;

		foreach (var effect in _activeEffects)
		{
			if (gameplayEffect == effect.GameplayEffectEvaluatedData.GameplayEffect)
			{
				effect.Unapply();
				effectToRemove = effect;
				break;
			}
		}

		if (effectToRemove is not null)
		{
			_activeEffects.Remove(effectToRemove);
		}
	}

	public void UnapplyEffectData(GameplayEffectData gameplayEffectData)
	{
		ActiveGameplayEffect? effectToRemove = null;

		foreach (var effect in _activeEffects)
		{
			if (gameplayEffectData == effect.GameplayEffectEvaluatedData.GameplayEffect.EffectData)
			{
				effect.Unapply();
				effectToRemove = effect;
				break;
			}
		}

		if (effectToRemove is not null)
		{
			_activeEffects.Remove(effectToRemove);
		}
	}

	public void UpdateEffects(double deltaTime)
	{
		foreach (var effect in _activeEffects)
		{
			effect.Update(deltaTime);
		}

		_activeEffects.RemoveAll(e => e.IsExpired);
	}

	public List<StackData> GetEffectStackCount(GameplayEffectData effectData)
	{
		List<StackData> stackDataList = new();

		// Stack by target, aggregate levels => 1 stack, easy
		// Stack by target, segregate levels => 1 stack per level
		// Stack by source, aggregate levels => 1 stack per instigator
		// stack by source, segregate levels => 1 stack per level per instigator

		foreach (var effect in _activeEffects)
		{
			if (effectData == effect.GameplayEffectEvaluatedData.GameplayEffect.EffectData)
			{
				stackDataList.Add(new StackData
				{
					Instigator = effect.GameplayEffectEvaluatedData.GameplayEffect.Context.Instigator,
					EffectLevel = effect.GameplayEffectEvaluatedData.Level,
					StackCount = effect.GameplayEffectEvaluatedData.Stack,
				});
			}
		}

		// No stack => 1 instance per application

		return stackDataList;
	}
}

public struct StackData
{
	public IForgeEntity Instigator;
	public int EffectLevel;
	public int StackCount;
}
