using System;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Locking;
using Sitecore.Links;
using Sitecore.NSubstituteUtils;
using Sitecore.Security.AccessControl;
using Xunit;

namespace Sitecore.NSubstitute.UnitTests
{
    public class FakeItemTester
    {
        [Fact]
        public void Constructor_WhenCalled_DoesNotThrow()
        {
            // Act
            Action callConstructor = () =>
            {
                var instance = new FakeItem();
                MainUtil.Nop(instance);
            };

            // Assert
            callConstructor.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WhenCalled_SetsNonEmptyID()
        {
            // Act
            var fakeItem = new FakeItem();

            // Assert
            fakeItem.ID
                .Should().NotBeNull()
                .And.NotBe(ID.Null);
        }

        [Fact]
        public void ToSitecoreItem_WhenCalled_ReturnsRealItem()
        {
            // Arrange
            var fakeItem = new FakeItem();

            // Act
            var item = fakeItem.ToSitecoreItem();

            // Assert
            item.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCastedToSitecoreItem_ReturnsInstance()
        {
            // Arrange
            var fakeItem = new FakeItem();

            // Act 
            var item = (Item)fakeItem;

            // Assert
            item.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_ShouldInitialize_ItemProperties()
        {
            Item item = new FakeItem();

            item.Fields.Should().NotBeNull();
            item.Template.Should().NotBeNull();
            item.Children.Should().NotBeNull();
            item.Children.Count.Should().Be(0);
            item.Database.Should().NotBeNull();
            item.Database.GetItem(item.ID).Should().Be(item);
            item.Language.Should().Be(Globalization.Language.Invariant);
            item.Version.Should().Be(Sitecore.Data.Version.First);
        }

        [Theory, InlineAutoData("test field", "test value")]
        public void FieldIndexerByFieldId_AfterAdd_ReturnsAddedValue(string fieldName, string addedFieldValue, ID fieldId)
        {
            // Arrange
            var fakeItem = new FakeItem();

            fakeItem.Add(fieldId, fieldName, addedFieldValue);
            Item item = fakeItem;

            // Act
            var actualFieldValue = item.Fields[fieldId].Value;

            // Assert
            actualFieldValue.Should().Be(addedFieldValue);
        }

        [Theory, InlineAutoData("test field", "test value")]
        public void ItemIndexerByFieldId_AfterAdd_ReturnsAddedValue(string fieldName, string addedFieldValue, ID fieldId)
        {
            // Arrange
            var fakeItem = new FakeItem();

            fakeItem.Add(fieldId, fieldName, addedFieldValue);
            Item item = fakeItem;

            // Act
            var actualFieldValue = item[fieldId];

            // Assert
            actualFieldValue.Should().Be(addedFieldValue);
        }


        [Theory, InlineAutoData("test field", "test value")]
        public void FieldIndexerByFieldName_AfterAdd_ReturnsAddedValue(string fieldName, string addedFieldValue, ID fieldId)
        {
            // Arrange
            var fakeItem = new FakeItem();

            fakeItem.Add(fieldId, fieldName, addedFieldValue);
            Item item = fakeItem;

            // Act
            var actualFieldValue = item.Fields[fieldName].Value;

            // Assert
            actualFieldValue.Should().Be(addedFieldValue);
        }

        [Theory, InlineAutoData("test field", "test value")]
        public void ItemIndexerByFieldName_AfterAdd_ReturnsAddedValue(string fieldName, string addedFieldValue, ID fieldId)
        {
            // Arrange
            var fakeItem = new FakeItem();

            fakeItem.Add(fieldId, fieldName, addedFieldValue);
            Item item = fakeItem;

            // Act
            var actualFieldValue = item[fieldName];

            // Assert
            actualFieldValue.Should().Be(addedFieldValue);
        }

        [Theory, InlineAutoData("test field", "test value")]
        public void FieldIndexerByFieldName_AfterAddByName_ReturnsAddedValue(string fieldName, string addedFieldValue)
        {
            // Arrange
            var fakeItem = new FakeItem();

            fakeItem.Add(fieldName, addedFieldValue);
            Item item = fakeItem;

            // Act
            var actualFieldValue = item.Fields[fieldName].Value;

            // Assert
            actualFieldValue.Should().Be(addedFieldValue);
        }

        [Theory, InlineAutoData("test value")]
        public void ItemIndexerByFieldName_AfterAddByFieldId_ReturnsAddedValue(string addedFieldValue, ID fieldId)
        {
            // Arrange
            var fakeItem = new FakeItem();

            fakeItem.Add(fieldId, addedFieldValue);
            Item item = fakeItem;

            // Act
            var actualFieldValue = item[fieldId];

            // Assert
            actualFieldValue.Should().Be(addedFieldValue);
        }

        [Theory, InlineAutoData("test value")]
        public void ItemIndexerByFieldName_AfterAddByFieldId_ReturnsEmptyName(string addedFieldValue, ID fieldId)
        {
            // Arrange
            var fakeItem = new FakeItem();

            fakeItem.Add(fieldId, addedFieldValue);
            Item item = fakeItem;

            // Act
            var fieldName = item.Fields[fieldId].Name;

            // Assert
            fieldName.Should().BeEmpty();
        }

        [Theory, AutoData]
        public void WithParent_WhenCalled_SetsParentID(ID parentId)
        {
            // Act
            Item item = new FakeItem().WithParent(new FakeItem(parentId));

            // Assert
            item.ParentID.Should().Be(parentId);
        }

        [Theory, AutoData]
        public void FakeItem_ShouldSimplify_StructureCreation(ID firstChild, ID secondChild)
        {
            var item = new FakeItem();

            var scItem = (Item)item;
            item
              .WithChild(new FakeItem(firstChild, scItem.Database))
              .WithChild(new FakeItem(secondChild, scItem.Database));

            scItem.Children.Should().HaveCount(2);

            scItem.Database.GetItem(firstChild).Should().NotBeNull();
            scItem.Database.GetItem(firstChild).ID.Should().Be(firstChild);
        }

        [Fact]
        public void FakeItem_ShouldInitialize_Template()
        {
            var templateId = ID.NewID;
            var item = new FakeItem().WithTemplate(templateId);

            var scItem = (Item)item;
            scItem.Template.Should().NotBeNull();
            scItem.TemplateID.Should().Be(templateId);
            scItem.Template.ID.Should().Be(templateId);
            scItem.Database.GetTemplate(templateId).Should().NotBeNull();
            scItem.Database.Engines.TemplateEngine.GetTemplate(templateId).Should().NotBeNull();
        }

        [Theory, InlineData("my test item")]
        [AutoData]
        public void WithName_WhenCalled_SetsItemName(string itemName)
        {
            Item item = new FakeItem()
                .WithName(itemName);

            item.Name.Should().Be(itemName);
        }

        [Fact]
        public void HasChildren_WhenNoChildrenAdded_ReturnsFalse()
        {
            Item item = new FakeItem();

            item.HasChildren.Should().BeFalse();
        }

        [Fact]
        public void HasChildren_WhenChildAdded_ReturnsTrue()
        {
            Item item = new FakeItem().WithChild(new FakeItem());

            item.HasChildren.Should().BeTrue();
        }

        [Theory, AutoData]
        public void Add_WhenChildAdded_ChildrenCountIncrements(int childrenToAdd)
        {
            var reducedChildCount = (childrenToAdd % 9) + 4;
            var fakeItem = new FakeItem();

            Enumerable.Repeat(0, reducedChildCount)
                .ToList()
                .ForEach(_ => fakeItem.Add(new FakeItem()));

            Item item = fakeItem;
            item.Children.Should().HaveCount(reducedChildCount);
        }

        [Fact]
        public void FakeItem_WhenCreated_DefinesItemPaths()
        {
            Item item = new FakeItem();

            item.Paths.Should().NotBeNull();
        }

        [Theory, InlineData("/test/somepath")]
        [AutoData]
        public void WithPath_WhenFullPathRequested_ReturnsItemPath(string itemPath)
        {             
            Item item = new FakeItem().WithPath(itemPath);

            item.Paths.FullPath.Should().Be(itemPath);
        }

        [Theory, InlineData("/test/somepath")]
        [AutoData]
        public void WithPath_WhenGetItemCalledWithConfiguredPath_ReturnsItem(string itemPath)
        {
            // Arrange
            Item item = new FakeItem().WithPath(itemPath);

            // Act
            var itemByPath = item.Database.GetItem(itemPath);

            // Assert
            itemByPath.Should().Be(item);
        }

        [Fact]
        public void FakeItem_ShouldFake_ItemPathSeveralTimes()
        {
            string itemPath1 = "/test/somepath";
            string itemPath2 = "/test/somepath";
            var fakeItem = new FakeItem().WithPath(itemPath1);

            fakeItem.ToSitecoreItem().Paths.FullPath.Should().Be(itemPath1);
            fakeItem.WithPath(itemPath2);
            fakeItem.ToSitecoreItem().Paths.FullPath.Should().Be(itemPath2);
            fakeItem.ToSitecoreItem().Database.GetItem(itemPath2).Should().Be(fakeItem.ToSitecoreItem());
        }

        [Fact]
        public void FakeItem_ShouldBePossible_FakeItemAccess()
        {
            var fakeItem = new FakeItem();

            fakeItem.ToSitecoreItem().Access.Should().BeNull();

            fakeItem.WithItemAccess();
            fakeItem.ToSitecoreItem().Access.Should().NotBeNull();

            var item = fakeItem.ToSitecoreItem();
            item.Access.CanRead().Returns(true);

            item.Access.CanRead().Should().BeTrue();
        }

        [Fact]
        public void FakeItem_ShouldFake_LanguageByName()
        {
            var fakeItem = new FakeItem()
                            .WithLanguage("en-US");

            var item = fakeItem.ToSitecoreItem();
            item.Language.Should().NotBeNull();
            item.Language.Name.Should().Be("en-US");
            item.Database.GetItem(item.ID, item.Language).Should().Be(item);
        }

        [Fact]
        public void FakeItem_ShouldFake_LanguageByNameUsingStringID()
        {
            var fakeItem = new FakeItem()
                            .WithLanguage("en-US");

            var item = fakeItem.ToSitecoreItem();
            item.Language.Should().NotBeNull();
            item.Language.Name.Should().Be("en-US");
            item.Database.GetItem(item.ID.ToString(), item.Language).Should().Be(item);
        }

        [Fact]
        public void FakeItem_ShouldFake_LanguageObject()
        {
            var fakeItem = new FakeItem()
                            .WithLanguage(Sitecore.Globalization.Language.Parse("en-US"));

            var item = fakeItem.ToSitecoreItem();
            item.Language.Should().NotBeNull();
            item.Language.Name.Should().Be("en-US");
            item.Database.GetItem(item.ID, item.Language).Should().Be(item);
        }

        [AutoData]
        [Theory, InlineData("/test/path", 4)]
        public void FakeItem_ShouldFake_Uri(string itemPath, int version)
        {
            var fakeItem = new FakeItem()
              .WithPath(itemPath)
              .WithLanguage("da")
              .WithVersion(version)
              .WithUri();

            Item item = fakeItem;
            item.Uri.Should().NotBeNull("uri");
            item.Uri.ItemID.Should().Be(item.ID);
            item.Uri.Path.Should().Be(item.Paths.FullPath);
            item.Uri.DatabaseName.Should().Be(item.Database.Name);
            item.Uri.Language.Should().Be(item.Language);
            item.Uri.Version.Should().Be(item.Version);
            item.Database.GetItem(item.Uri.ToDataUri()).Should().Be(item);
        }

        [Fact]
        public void FakeItem_WhenCreated_HasNoAppearance()
        {
            Item item = new FakeItem();

            item.Appearance.Should().BeNull();
        }

        [Fact]
        public void WithAppearance_WhenCalled_SetsAppearance()
        {
            // Arrange
            var fakeItem = new FakeItem();
            Item item = fakeItem;

            var appearance = Substitute.For<ItemAppearance>(item);

            // Act
            fakeItem.WithAppearance(appearance);

            item.Appearance.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_ShouldFake_Statistics()
        {
            var item = new FakeItem();

            item.ToSitecoreItem().Statistics.Should().BeNull();
            item.WithStatistics(Substitute.For<ItemStatistics>(item.ToSitecoreItem()));

            item.ToSitecoreItem().Statistics.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_ShouldFake_Links()
        {
            var item = new FakeItem();

            item.ToSitecoreItem().Links.Should().BeNull();
            item.WithItemLinks(Substitute.For<ItemLinks>(item.ToSitecoreItem()));

            item.ToSitecoreItem().Links.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_ShouldFake_Locking()
        {
            var item = new FakeItem();

            item.ToSitecoreItem().Locking.Should().BeNull();
            item.WithItemLocking(Substitute.For<ItemLocking>(item.ToSitecoreItem()));

            item.ToSitecoreItem().Locking.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_ShouldFake_Versions()
        {
            var item = new FakeItem();

            item.ToSitecoreItem().Versions.Should().BeNull();
            item.WithItemVersions(Substitute.For<ItemVersions>(item.ToSitecoreItem()));

            item.ToSitecoreItem().Versions.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_ShouldFake_Axes()
        {
            var fakeItem = new FakeItem();
            Item item = fakeItem;
            var stubItemAxes = Substitute.For<ItemAxes>(item);

            fakeItem.WithItemAxes(stubItemAxes);

            item.Axes
                .Should().NotBeNull()
                .And.Be(stubItemAxes);
        }

        [Fact]
        public void FakeItem_WhenCreated_HasNoAxes()
        {
            Item item = new FakeItem();

            item.Axes.Should().BeNull();
        }

        [Fact]
        public void FakeItem_ShouldFake_ItemEditing()
        {
            var item = new FakeItem();

            item.ToSitecoreItem().Editing.Should().BeNull();
            item.WithItemEditing(Substitute.For<ItemEditing>(item.ToSitecoreItem()));

            item.ToSitecoreItem().Editing.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_ShouldSupportExtending_WithExtensionMethods()
        {
            var item = new FakeItem();

            item.ToSitecoreItem().Help.Should().BeNull();

            item.WithItemHelp(Substitute.For<ItemHelp>(item.ToSitecoreItem()));

            item.ToSitecoreItem().Help.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_ItemAccess_SubstitutesObject()
        {
            var item = new FakeItem();
            var access = Substitute.For<ItemAccess>(item.ToSitecoreItem());
            item.WithItemAccess(access);

            item.ToSitecoreItem().Access.Should().Be(access);
        }

        [Fact]
        public void WithAppearance_WhenCalled_AddsItemAppearance()
        {
            Item item = new FakeItem().WithAppearance();

            item.Appearance.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_DefaultStatistics()
        {
            var item = new FakeItem()
              .WithStatistics()
              .ToSitecoreItem();

            item.Statistics.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_DefaultLinks()
        {
            var item = new FakeItem()
              .WithItemLinks()
              .ToSitecoreItem();

            item.Links.Should().NotBeNull();
        }

        [Fact]
        public void WithItemLocking_WhenCalled_ProvidesLocking()
        {
            Item item = new FakeItem().WithItemLocking();

            item.Locking.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_DefaultVersions()
        {
            var item = new FakeItem()
              .WithItemVersions()
              .ToSitecoreItem();

            item.Versions.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_DefaultAxes()
        {
            var item = new FakeItem()
              .WithItemAxes()
              .ToSitecoreItem();

            item.Axes.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_DefaultEditing()
        {
            Item item = new FakeItem().WithItemEditing();

            item.Editing.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_DefaultBranch()
        {
            Item item = new FakeItem().WithBranch();

            item.Branch.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void FakeItem_Substitutes_BranchId(ID branchId)
        {
            Item item = new FakeItem().WithBranchId(branchId);

            item.BranchId.Should().Be(branchId);
        }

        [Fact]
        public void FakeItem_Substitutes_Branches()
        {
            var branches = Array.Empty<BranchItem>();
            Item item = new FakeItem()
                .WithBranches(branches);

            item.Branches.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_Substitutes_Created()
        {
            var created = DateTime.UtcNow;
            Item item = new FakeItem().WithCreated(created);

            item.Created.Should().Be(created);
        }

        [Theory, InlineData("display name")]
        public void FakeItem_Substitutes_DisplayName(string displayName)
        {
            Item item = new FakeItem().WithDisplayName(displayName);

            item.DisplayName.Should().Be(displayName);
        }

        [Fact]
        public void FakeItem_Substitutes_GetNonEmptyClones()
        {
            var clones = new Item[] { new FakeItem().ToSitecoreItem() };
            Item item = new FakeItem().WithGetClones(clones);

            item.GetClones().Count().Should().Be(clones.Length);
            item.HasClones.Should().Be(clones.Length > 0);
        }

        [Fact]
        public void FakeItem_Substitutes_GetEmptyClones()
        {
            var clones = Array.Empty<Item>();
            Item item = new FakeItem()
              .WithGetClones(clones);

            item.GetClones().Count().Should().Be(clones.Length);
            item.HasClones.Should().Be(clones.Length > 0);
        }

        [Fact]
        public void FakeItem_DefaultHelp()
        {
            Item item = new FakeItem().WithHelp();

            item.Help.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_Substitutes_IsClone()
        {
            Item item = new FakeItem().WithIsClone(true);

            item.IsClone.Should().BeTrue();
        }

        [Fact]
        public void FakeItem_Substitutes_IsFallback()
        {
            var item = new FakeItem()
              .WithIsFallback(true)
              .ToSitecoreItem();

            item.IsFallback.Should().BeTrue();
        }

        [Fact]
        public void FakeItem_Substitutes_Languages()
        {
            var item = new FakeItem()
              .WithLanguages(new[] { "en", "da" })
              .ToSitecoreItem();

            item.Languages.Length.Should().Be(2);
            item.Languages.Any(x => x.Name == "en").Should().BeTrue();
            item.Languages.Any(x => x.Name == "da").Should().BeTrue();
        }

        [Fact]
        public void FakeItem_Substitutes_Modified()
        {
            var item = new FakeItem()
              .WithModified(true)
              .ToSitecoreItem();

            item.Modified.Should().BeTrue();
        }

        [Theory, AutoData]
        public void FakeItem_Substitutes_OriginatorId(ID originatorId)
        {
            Item item = new FakeItem()
                .WithOriginatorId(originatorId);

            item.OriginatorId.Should().Be(originatorId);
        }

        [Theory, InlineData("da")]
        public void FakeItem_Substitutes_OriginalLanguage(string originalLanguage)
        {
            Item item = new FakeItem()
              .WithOriginalLanguage(originalLanguage);

            item.OriginalLanguage.Name.Should().Be(originalLanguage);
        }

        [Fact]
        public void FakeItem_Substitutes_OriginalPublishing()
        {
            var item = new FakeItem()
              .WithPublishing()
              .ToSitecoreItem();

            item.Publishing.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_Substitutes_RuntimeSettings()
        {
            Item item = new FakeItem()
              .WithRuntimeSettings();

            item.RuntimeSettings.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_Substitutes_Security()
        {
            Item item = new FakeItem()
              .WithSecurity();

            item.Security.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_Substitutes_Source()
        {
            Item source = new FakeItem();
            Item item = new FakeItem().WithSource(source);

            item.Source.Should().Be(source);
        }

        [Fact]
        public void FakeItem_Substitutes_SourceUri()
        {
            Item item = new FakeItem().WithSourceUri();

            item.SourceUri.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_Substitutes_State()
        {
            Item item = new FakeItem().WithState();

            item.State.Should().NotBeNull();
        }

        [Theory, AutoData]
        [InlineData("template name")]
        public void FakeItem_Substitutes_TemplateName(string templateName)
        {
            Item item = new FakeItem()
              .WithTemplateName(templateName);

            item.TemplateName.Should().Be(templateName);
        }

        [Fact]
        public void FakeItem_Substitutes_Visualizations()
        {
            Item item = new FakeItem()
              .WithVisualization();

            item.Visualization.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_Substitutes_IsItemClone()
        {
            Item item = new FakeItem().WithIsItemClone(true);

            item.IsItemClone.Should().BeTrue();
        }

        [Fact]
        public void FakeItem_Substitutes_SharedFieldSource()
        {
            var source = new FakeItem().ToSitecoreItem();
            var item = new FakeItem()
              .WithSharedFieldsSource(source)
              .ToSitecoreItem();

            item.SharedFieldsSource.Should().Be(source);
        }
    }
}
