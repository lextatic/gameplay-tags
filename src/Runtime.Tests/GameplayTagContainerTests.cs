#pragma warning disable SA1600 // Elements should be documented
namespace GameplayTags.Runtime.Tests;

[TestClass]
public class GameplayTagContainerTests
{
	[TestMethod]
	[TestCategory("HasTag")]
	public void Container_should_have_tag()
	{
		var tagContainer = new GameplayTagContainer();
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		var tag = GameplayTag.RequestGameplayTag(TagName.FromString("A"));

		Assert.IsTrue(tagContainer.HasTag(tag));
	}

	[TestMethod]
	[TestCategory("HasTag")]
	public void Container_should_not_have_tag()
	{
		var tagContainer = new GameplayTagContainer();
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A")));
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		var tag = GameplayTag.RequestGameplayTag(TagName.FromString("A.1"));

		Assert.IsFalse(tagContainer.HasTag(tag));
	}

	[TestMethod]
	[TestCategory("HasTagExact")]
	public void Container_should_have_tag_exact()
	{
		var tagContainer = new GameplayTagContainer();
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		var tag = GameplayTag.RequestGameplayTag(TagName.FromString("A.1"));

		Assert.IsTrue(tagContainer.HasTagExact(tag));
	}

	[TestMethod]
	[TestCategory("HasTagExact")]
	public void Container_should_not_have_tag_exact()
	{
		var tagContainer = new GameplayTagContainer();
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		var tag = GameplayTag.RequestGameplayTag(TagName.FromString("A"));

		Assert.IsFalse(tagContainer.HasTagExact(tag));
	}

	[TestMethod]
	[TestCategory("HasAny")]
	public void Container_A_should_have_any_container_B_tag()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		Assert.IsTrue(tagContainerA.HasAny(tagContainerB));
	}

	[TestMethod]
	[TestCategory("HasAny")]
	public void Container_A_should_not_have_any_container_B_tag()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		Assert.IsFalse(tagContainerA.HasAny(tagContainerB));
	}

	[TestMethod]
	[TestCategory("HasAnyExact")]
	public void Container_A_should_have_any_container_B_tag_exact()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		Assert.IsTrue(tagContainerA.HasAnyExact(tagContainerB));
	}

	[TestMethod]
	[TestCategory("HasAnyExact")]
	public void Container_A_should_not_have_any_container_B_tag_exact()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		Assert.IsFalse(tagContainerA.HasAnyExact(tagContainerB));
	}

	[TestMethod]
	[TestCategory("HasAll")]
	public void Container_A_should_have_all_container_B_tags()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		Assert.IsTrue(tagContainerA.HasAll(tagContainerB));
	}

	[TestMethod]
	[TestCategory("HasAll")]
	public void Container_A_should_not_have_all_container_B_tags()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		Assert.IsFalse(tagContainerA.HasAll(tagContainerB));
	}

	[TestMethod]
	[TestCategory("HasAllExact")]
	public void Container_A_should_have_all_container_B_tags_exact()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		Assert.IsTrue(tagContainerA.HasAllExact(tagContainerB));
	}

	[TestMethod]
	[TestCategory("HasAllExact")]
	public void Container_A_should_not_have_all_container_B_tags_exact()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		Assert.IsFalse(tagContainerA.HasAllExact(tagContainerB));
	}

	[TestMethod]
	[TestCategory("Filter")]
	public void Container_A_filter_container_B_should_have_tags_A1_and_B1_only()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Color.Red")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		var tagContainerC = tagContainerA.Filter(tagContainerB);

		var validationContainer = new GameplayTagContainer();
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		Assert.IsTrue(tagContainerC.HasAllExact(validationContainer));
		Assert.IsTrue(tagContainerC.Count == 2);
	}

	[TestMethod]
	[TestCategory("FilterExact")]
	public void Container_A_filter_exact_container_B_should_not_have_tag_A1_only()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Color.Red")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		var tagContainerC = tagContainerA.FilterExact(tagContainerB);

		var validationContainer = new GameplayTagContainer();
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));

		Assert.IsTrue(tagContainerC.HasAllExact(validationContainer));
		Assert.IsTrue(tagContainerC.Count == 1);
	}

	[TestMethod]
	[TestCategory("Append")]
	public void Container_A_append_container_B_should_have_all_tags_from_both()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Color.Red")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		tagContainerA.AppendTags(tagContainerB);

		var validationContainer = new GameplayTagContainer();
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("1")));
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Color.Red")));
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));

		Assert.IsTrue(tagContainerA.HasAllExact(validationContainer));
		Assert.IsTrue(tagContainerA.Count == 5);
	}

	[TestMethod]
	[TestCategory("AppendMatchingTags")]
	public void Container_A_append_matching_tags_container_B_should_add_b1_only()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Color.Red")));

		var tagContainerB = new GameplayTagContainer();
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));
		tagContainerB.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Color.Blue")));

		var tagContainerC = new GameplayTagContainer();
		tagContainerC.AppendMatchingTags(tagContainerA, tagContainerB);

		var validationContainer = new GameplayTagContainer();
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		validationContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));

		Assert.IsTrue(tagContainerC.HasAllExact(validationContainer));
		Assert.IsTrue(tagContainerC.Count == 2);
	}

	[TestMethod]
	[TestCategory("MatchesQuery")]
	public void Container_matches_query_should_match()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Color.Red")));

		var query = new GameplayTagQuery();
		query.Build(new GameplayTagQueryExpression()
			.AllTagsMatch()
			.AddTag("A.1")
			.AddTag("B.1"));

		Assert.IsTrue(tagContainerA.MatchesQuery(query));
	}

	[TestMethod]
	[TestCategory("MatchesQuery")]
	public void Container_matches_query_shouldnt_match()
	{
		var tagContainerA = new GameplayTagContainer();
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("B")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("1")));
		tagContainerA.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Color.Red")));

		var query = new GameplayTagQuery();
		query.Build(new GameplayTagQueryExpression()
			.AllTagsMatch()
			.AddTag("A.1")
			.AddTag("B.1"));

		Assert.IsFalse(tagContainerA.MatchesQuery(query));
	}

	[TestMethod]
	[TestCategory("Serialization")]
	public void Container_should_serialize_successfully()
	{
		var tagContainer = new GameplayTagContainer();
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.B.1")));
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.B.2")));
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.C.3")));
		tagContainer.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Character.Attributes.Dex")));

		GameplayTagContainer.NetSerialize(tagContainer, out var containerStream);

		Assert.IsTrue(containerStream[0] == 0);
		Assert.IsTrue(containerStream.Length == 12);
		Assert.IsTrue(containerStream.SequenceEqual(new byte[] { 0, 5, 2, 0, 4, 0, 5, 0, 9, 0, 14, 0 }));
	}

	[TestMethod]
	[TestCategory("Serialization")]
	public void Container_should_serialize_empty_container_successfully()
	{
		var tagContainer = new GameplayTagContainer();

		GameplayTagContainer.NetSerialize(tagContainer, out var containerStream);

		Assert.IsTrue(containerStream[0] == 1);
		Assert.IsTrue(containerStream.Length == 1);
	}

	[TestMethod]
	[TestCategory("Serialization")]
	public void Container_should_deserialize_successfully()
	{
		Assert.IsTrue(GameplayTagContainer.NetDeserialize(
			[0, 5, 2, 0, 4, 0, 5, 0, 9, 0, 14, 0],
			out var deserializedContainer));

		var tagContainerCheck = new GameplayTagContainer();
		tagContainerCheck.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.1")));
		tagContainerCheck.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.B.1")));
		tagContainerCheck.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.B.2")));
		tagContainerCheck.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("A.C.3")));
		tagContainerCheck.AddTag(GameplayTag.RequestGameplayTag(TagName.FromString("Character.Attributes.Dex")));

		Assert.IsTrue(deserializedContainer == tagContainerCheck);
	}
}
