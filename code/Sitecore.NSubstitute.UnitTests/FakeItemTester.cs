using System;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Templates;
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
        public void FakeItem_WhenCastedToSitecoreItem_ReturnsItemInstance()
        {
            // Arrange
            var fakeItem = new FakeItem();

            // Act 
            var item = (Item)fakeItem;

            // Assert
            item.Should().NotBeNull();
        }
        #region Default creation
        [Fact]
        public void Constructor_WhenCalled_HasItemFields()
        {
            Item item = new FakeItem();

            item.Fields.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WhenCalled_HasTemplateSet()
        {
            Item item = new FakeItem();


            item.Template.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WhenCalled_HasChildrenPropertyInitialized()
        {
            Item item = new FakeItem();

            item.Children.Should().NotBeNull();
        }

        [Theory]
        [InlineData(ChildListOptions.None)]
        [InlineData(ChildListOptions.AllowReuse)]
        [InlineData(ChildListOptions.IgnoreSecurity)]
        [InlineData(ChildListOptions.SkipSorting)]
        public void Constructor_WhenCalled_GetChildrenMethodIsSetupInitialized(ChildListOptions options)
        {
            Item item = new FakeItem();

            item.GetChildren().Should().NotBeNull();
            item.GetChildren(options).Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WhenCalled_HasZeroChildren()
        {
            Item item = new FakeItem();

            item.Children.Should().BeEmpty();
        }

        [Theory]
        [InlineData(ChildListOptions.None)]
        [InlineData(ChildListOptions.AllowReuse)]
        [InlineData(ChildListOptions.IgnoreSecurity)]
        [InlineData(ChildListOptions.SkipSorting)]
        public void Constructor_WhenCalled_HasNoChildren(ChildListOptions options)
        {
            Item item = new FakeItem();

            item.GetChildren().Should().BeEmpty();
            item.GetChildren(options).Should().BeEmpty();
        }

        [Fact]
        public void Constructor_WhenCalled_HasDatabaseSet()
        {
            Item item = new FakeItem();

            item.Database.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void Constructor_WhenCalled_ConfiguresDatabaseToFindSelfById(ID itemId)
        {
            Item item = new FakeItem(itemId);

            var actualItem = item.Database.GetItem(itemId);

            actualItem.Should().BeSameAs(item);
        }

        [Fact]
        public void Constructor_WhenCalled_HasInvariantLanguage()
        {
            Item item = new FakeItem();

            item.Language.Should().Be(Globalization.Language.Invariant);
        }

        [Fact]
        public void Constructor_WhenCalled_HasFirstVersion()
        {
            Item item = new FakeItem();

            item.Version.Should().Be(Sitecore.Data.Version.First);
        }
        #endregion

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
        public void WithChild_WhenCalled_CreatesChildItems(ID firstChild, ID secondChild)
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

        [Theory, AutoData]
        public void WithChild_WhenCalled_UpdatesGetChildren(ID firstChild, ID secondChild)
        {
            var item = new FakeItem();

            var scItem = (Item)item;
            item
                .WithChild(new FakeItem(firstChild, scItem.Database))
                .WithChild(new FakeItem(secondChild, scItem.Database));

            scItem.GetChildren().Should().HaveCount(2);
            scItem.GetChildren(ChildListOptions.None).Should().HaveCount(2);
        }

        #region WithTemplate tests

        [Theory, AutoData]
        public void WithTemplate_WhenCalled_SetsItemTemplateIdToConfigured(ID templateId)
        {
            Item item = new FakeItem().WithTemplate(templateId);

            item.Template.ID.Should().Be(templateId);
        }

        [Theory, AutoData]
        public void WithTemplate_WhenCalled_CreatesItemTemplate(ID templateId)
        {
            Item item = new FakeItem().WithTemplate(templateId);

            item.Template.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void WithTemplate_WhenCalled_ConfiguresItemDatabaseToFindItemTemplate(ID templateId)
        {
            // Arrange
            Item item = new FakeItem().WithTemplate(templateId);

            // Act
            var itemTemplate = item.Database.GetTemplate(templateId);

            // Assert
            itemTemplate.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void WithTemplate_WhenCalled_ConfiguresTemplateEngineToFindItemTemplate(ID templateId, string contextDatabaseName)
        {
            // Arrange
            var database = FakeUtil.FakeDatabase(contextDatabaseName);
            Item item = new FakeItem(database: database).WithTemplate(templateId);

            // Act
            var itemTemplate = database.Engines.TemplateEngine.GetTemplate(templateId);

            // Assert
            itemTemplate
                .Should().NotBeNull()
                .And.Match<Template>(template => template.ID == templateId);
        }

        [Theory, AutoData]
        public void WithTemplate_WhenCalledMultipleTimesForSameDatabase_AllowsLocatingAny(string contextDatabaseName, ID firstTemplateId, ID secondTemplateId)
        {
            // Arrange
            var database = FakeUtil.FakeDatabase(contextDatabaseName);

            Item unoItem = new FakeItem(database: database).WithTemplate(firstTemplateId);
            Item dosItem = new FakeItem(database: database).WithTemplate(secondTemplateId);

            // Act
            var unoTemplate = database.Engines.TemplateEngine.GetTemplate(firstTemplateId);

            // Assert
            unoTemplate
                .Should().NotBeNull()
                .And.Match<Template>(template => template.ID == firstTemplateId);
        }

        #endregion

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

        [Theory, AutoData]
        public void Add_WhenChildAdded_GetChildrenMethodChildrenCountIncrements(int childrenToAdd)
        {
            var reducedChildCount = (childrenToAdd % 9) + 4;
            var fakeItem = new FakeItem();

            Enumerable.Repeat(0, reducedChildCount)
                .ToList()
                .ForEach(_ => fakeItem.Add(new FakeItem()));

            Item item = fakeItem;
            item.GetChildren().Should().HaveCount(reducedChildCount);
            item.GetChildren(ChildListOptions.None).Should().HaveCount(reducedChildCount);
        }

        [Fact]
        public void Constructor_WhenCalled_DefinesItemPaths()
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

        [Theory, InlineData("/test/somepath", "/test/anotherPath")]
        public void WithPath_WhenCalledMultipleTime_PreservesLaterPath(string originalPath, string laterPath)
        {
            // Arrange
            Item item = new FakeItem()
                .WithPath(originalPath)
                .WithPath(laterPath);

            // Act
            var actualFullPath = item.Paths.FullPath;

            // Assert
            actualFullPath.Should().Be(laterPath);
        }

        [Theory, InlineData("/test/somepath", "/test/anotherPath")]
        public void WithPath_WhenCalledMultipleTime_ConfiguresDatabaseToFindItemByLaterPath(string originalPath, string laterPath)
        {
            // Arrange
            Item item = new FakeItem()
                .WithPath(originalPath)
                .WithPath(laterPath);

            // Act
            var actualItem = item.Database.GetItem(laterPath);

            // Assert
            actualItem.Should().BeSameAs(item);
        }

        [Fact]
        public void Constructor_WhenCalled_DoesNotCreateItemAccess()
        {
            Item item = new FakeItem();

            item.Access.Should().BeNull();
        }

        [Fact]
        public void WithItemAccess_WhenCalled_LetsMockItemAccess()
        {
            // Arrange
            Item item = new FakeItem().WithItemAccess();

            item.Access.CanRead().Returns(true);

            // Act
            var actualCanRead = item.Access.CanRead();

            // Assert
            actualCanRead.Should().BeTrue();
        }

        [Theory, InlineData("en-US")]
        public void WithLanguage_WhenCalledWithLanguageName_SetsItemLanguage(string languageName)
        {
            Item item = new FakeItem().WithLanguage(languageName);

            item.Language.Name.Should().Be(languageName);
        }

        [Theory, InlineData("en-US")]
        public void WithLanguage_WhenCalledWithLanguageName_ConfiguresDatabaseToFindItemByIdInLanguage(string languageName)
        {
            Item item = new FakeItem().WithLanguage(languageName);

            var actualItem = item.Database.GetItem(item.ID, item.Language);

            actualItem.Should().Be(item);
        }

        [Theory, InlineData("en-US")]
        public void WithLanguage_WhenCalled_SetsItemLanguage(string languageName)
        {
            Item item = new FakeItem().WithLanguage(Globalization.Language.Parse(languageName));

            item.Language.Name.Should().Be(languageName);
        }

        [Theory, InlineData("en-US")]
        public void WithLanguage_WhenCalled_ConfiguresDatabaseToFindItemByIdInLanguage(string languageName)
        {
            Item item = new FakeItem().WithLanguage(Globalization.Language.Parse(languageName));

            var actualItem = item.Database.GetItem(item.ID, item.Language);

            actualItem.Should().Be(item);
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
        public void Constructor_WhenCalled_HasNoAppearance()
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

            // Assert
            item.Appearance.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WhenCalled_HasNoStatistics()
        {
            Item item = new FakeItem();

            item.Statistics.Should().BeNull();
        }

        [Fact]
        public void WithStatistics_WhenCalled_CreatesStatistics()
        {
            var item = new FakeItem();

            item.WithStatistics();

            item.ToSitecoreItem().Statistics.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WhenCalled_HasNoLinks()
        {
            Item item = new FakeItem();

            item.Links.Should().BeNull();
        }

        [Fact]
        public void Constructor_WhenCalled_HasNoLocking()
        {
            Item item = new FakeItem();

            item.Locking.Should().BeNull();
        }

        [Fact]
        public void WithItemVersions_WhenCalled_SetsItemVersions()
        {
            var item = new FakeItem();

            item.ToSitecoreItem().Versions.Should().BeNull();
            item.WithItemVersions(Substitute.For<ItemVersions>(item.ToSitecoreItem()));

            item.ToSitecoreItem().Versions.Should().NotBeNull();
        }

        [Fact]
        public void WithItemAxes_WhenCalled_SetsItemAxes()
        {
            var fakeItem = new FakeItem();
            Item item = fakeItem;
            var stubItemAxes = Substitute.For<ItemAxes>(item);

            fakeItem.WithItemAxes(stubItemAxes);

            item.Axes.Should().Be(stubItemAxes);
        }

        [Fact]
        public void Constructor_WhenCalled_HasNoAxes()
        {
            Item item = new FakeItem();

            item.Axes.Should().BeNull();
        }

        [Fact]
        public void Constructor_WhenCalled_DoesNotCreateHelp()
        {
            Item item = new FakeItem();

            item.Help.Should().BeNull();
        }

        [Fact]
        public void WithHelp_WhenCalled_CreatesItemHelp()
        {
            Item item = new FakeItem().WithHelp();

            item.Help.Should().NotBeNull();
        }

        [Fact]
        public void WithItemAccess_WhenCalled_SetsItemAccess()
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
        public void WithStatistics_WhenCalled_CreatesItemStatistics()
        {
            Item item = new FakeItem().WithStatistics();

            item.Statistics.Should().NotBeNull();
        }

        [Fact]
        public void WithItemLinks_WhenCalled_CreatesItemLinks()
        {
            Item item = new FakeItem().WithItemLinks();

            item.Links.Should().NotBeNull();
        }

        [Fact]
        public void WithItemLocking_WhenCalled_ProvidesLocking()
        {
            Item item = new FakeItem().WithItemLocking();

            item.Locking.Should().NotBeNull();
        }

        [Fact]
        public void WithItemVersions_WhenCalled_CreatesItemVersions()
        {
            Item item = new FakeItem().WithItemVersions();

            item.Versions.Should().NotBeNull();
        }

        [Fact]
        public void WithItemAxes_WhenCalled_CreatesItemAxes()
        {
            Item item = new FakeItem().WithItemAxes();

            item.Axes.Should().NotBeNull();
        }

        [Fact]
        public void WithItemEditing_WhenCalled_CreatesItemEditing()
        {
            Item item = new FakeItem().WithItemEditing();

            item.Editing.Should().NotBeNull();
        }

        [Fact]
        public void WithBranch_WhenCalled_CreatesItemBranch()
        {
            Item item = new FakeItem().WithBranch();

            item.Branch.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void WithBranchId_WhenCalled_SetsItemBranchId(ID branchId)
        {
            Item item = new FakeItem().WithBranchId(branchId);

            item.BranchId.Should().Be(branchId);
        }

        [Fact]
        public void WithBranches_WhenCalled_SetsItemBranches()
        {
            var branches = new BranchItem[0];
            Item item = new FakeItem().WithBranches(branches);

            item.Branches.Should().NotBeNull();
        }

        [Fact]
        public void WithCreated_WhenCalled_SetsItemCreated()
        {
            var created = DateTime.UtcNow;
            Item item = new FakeItem().WithCreated(created);

            item.Created.Should().Be(created);
        }

        [Theory, InlineData("display name")]
        public void WithDisplayName_WhenCalled_SetsDisplayName(string displayName)
        {
            Item item = new FakeItem().WithDisplayName(displayName);

            item.DisplayName.Should().Be(displayName);
        }

        [Theory, AutoData]
        public void WithGetClones_WhenMultipleClones_ConfiguresHasClonesReturnTrue(int cloneCount)
        {
            var reducedCloneCount = cloneCount % 7 + 2;

            var clones = from _ in Enumerable.Range(0, reducedCloneCount)
                         let fakeClone = new FakeItem()
                         select fakeClone.ToSitecoreItem();

            // Arrange
            Item item = new FakeItem().WithGetClones(clones);

            // Act
            var hasClonesActual = item.HasClones;

            // Assert
            hasClonesActual.Should().BeTrue();
        }

        [Theory, AutoData]
        public void WithGetClones_WhenMultipleClones_GetClonesReturnedConfigured(int cloneCount)
        {
            var reducedCloneCount = cloneCount % 7 + 2;

            var clones = (from _ in Enumerable.Range(0, reducedCloneCount)
                          let fakeClone = new FakeItem()
                          select fakeClone.ToSitecoreItem())
                .ToList();

            // Arrange
            Item item = new FakeItem().WithGetClones(clones);

            // Act
            var actualClones = item.GetClones();

            // Assert
            actualClones
                .Should().BeSubsetOf(clones)
                .And.HaveSameCount(clones);
        }

        [Fact]
        public void WithGetClones_WhenCalled_ConfiguresItemGetClones()
        {
            var clones = new Item[0];
            Item item = new FakeItem().WithGetClones(clones);

            item.GetClones().Should().BeEmpty();
            item.HasClones.Should().BeFalse();
        }

        [Fact]
        public void WithGetClones_WhenCalled_ConfiguresHasClones()
        {
            var clones = new Item[0];
            Item item = new FakeItem().WithGetClones(clones);

            item.HasClones.Should().BeFalse();
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithIsClone_WhenCalled_SetsConfiguredValue(bool isCloneConfigured)
        {
            // Arrange
            Item item = new FakeItem().WithIsClone(isCloneConfigured);

            // Act
            var isCloneActual = item.IsClone;

            // Assert
            isCloneActual.Should().Be(isCloneConfigured);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithIsFallback_WhenCalled_SetsIsFallbackToConfiguredValue(bool isFallbackConfigured)
        {
            // Arrange
            Item item = new FakeItem().WithIsFallback(isFallbackConfigured);

            // Act
            var isFallbackActual = item.IsFallback;

            // Assert
            isFallbackActual.Should().Be(isFallbackConfigured);
        }

        [Theory, InlineData("en","da")]
        public void WithLanguages_WhenCalled_SetsItemLanguages(string unoLanguage, string dosLanguage)
        {
            Item item = new FakeItem().WithLanguages(new[] {unoLanguage, dosLanguage });

            item.Languages
                .Should().HaveCount(2)
                .And.ContainSingle(language => language.Name == unoLanguage)
                .And.ContainSingle(language => language.Name == dosLanguage);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithModified_WhenCalled_SetsModifiedToConfiguredValue(bool isModifiedConfigured)
        {
            Item item = new FakeItem().WithModified(isModifiedConfigured);

            var isModifiedActual = item.Modified;

            isModifiedActual.Should().Be(isModifiedConfigured);
        }

        [Theory, AutoData]
        public void WithOriginatorId_WhenCalled_SetsItemOriginationId(ID originatorId)
        {
            Item item = new FakeItem().WithOriginatorId(originatorId);

            item.OriginatorId.Should().Be(originatorId);
        }

        [Theory, InlineData("da")]
        public void WithOriginalLanguage_WhenCalled_SetsItemOriginalLanguage(string originalLanguage)
        {
            Item item = new FakeItem().WithOriginalLanguage(originalLanguage);

            item.OriginalLanguage.Name.Should().Be(originalLanguage);
        }

        [Fact]
        public void WithPublishing_WhenCalled_CreatesItemPublishing()
        {
            Item item = new FakeItem().WithPublishing();

            item.Publishing.Should().NotBeNull();
        }

        [Fact]
        public void WithRuntimeSettings_WhenCalled_CreatesItemRuntimeSettings()
        {
            Item item = new FakeItem().WithRuntimeSettings();

            item.RuntimeSettings.Should().NotBeNull();
        }

        [Fact]
        public void WithSecurity_WhenCalled_CreatesItemSecurity()
        {
            Item item = new FakeItem().WithSecurity();

            item.Security.Should().NotBeNull();
        }

        [Fact]
        public void WithSource_WhenCalled_SetsSourceToReturnConfiguredValue()
        {
            Item source = new FakeItem();
            Item item = new FakeItem().WithSource(source);

            item.Source.Should().Be(source);
        }

        [Fact]
        public void WithSourceUri_WhenCalled_SetsItemSourceUri()
        {
            Item item = new FakeItem().WithSourceUri();

            item.SourceUri.Should().NotBeNull();
        }

        [Fact]
        public void WithState_WhenCalled_CreatesItemState()
        {
            Item item = new FakeItem().WithState();

            item.State.Should().NotBeNull();
        }

        [Theory, AutoData]
        [InlineData("template name")]
        public void WithTemplateName_WhenCalled_SetsItemTemplateName(string templateName)
        {
            Item item = new FakeItem().WithTemplateName(templateName);

            item.TemplateName.Should().Be(templateName);
        }

        [Fact]
        public void WithVisualization_WhenCalled_CreatesItemVisualization()
        {
            Item item = new FakeItem().WithVisualization();

            item.Visualization.Should().NotBeNull();
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithIsItemClone_WhenCalled_SetsIsItemClone(bool isItemCloneConfigured)
        {
            Item item = new FakeItem().WithIsItemClone(isItemCloneConfigured);

            var actualIsItemClone = item.IsItemClone;

            actualIsItemClone.Should().Be(isItemCloneConfigured);
        }

        [Fact]
        public void WithSharedFieldsSource_WhenCalled_SetsSharedFieldsSource()
        {
            Item source = new FakeItem();

            Item item = new FakeItem().WithSharedFieldsSource(source);

            item.SharedFieldsSource.Should().Be(source);
        }
    }
}
