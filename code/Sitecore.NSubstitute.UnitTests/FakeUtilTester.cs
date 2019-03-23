using System;
using System.IO;
using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.NSubstituteUtils;
using Xunit;
using Version = Sitecore.Data.Version;

namespace Sitecore.NSubstitute.UnitTests
{
    public class FakeUtilTester
    {
        [AutoData]
        [Theory, InlineData("fake name")]
        public void FakeDatabase_WhenCalled_SetsDatabaseName(string databaseName)
        {
            var database = FakeUtil.FakeDatabase(databaseName);

            database.Name.Should().Be(databaseName);
        }
        
        [Theory, AutoData]
        public void FakeDatabase_WhenCalled_NeverReturnsNull(string databaseName)
        {
            var database = FakeUtil.FakeDatabase(databaseName);

            database
                .Should().NotBeNull()
                .And.BeAssignableTo<Database>();
        }

        [Fact]
        public void FakeDatabase_ShouldReturn_FakeDatabaseWithDefaultName()
        {
            var database = FakeUtil.FakeDatabase();

            database.Name.Should().Be("fakeDB");
        }

        [Theory, AutoData]
        public void FakeItem_WhenCalled_NeverReturnsNull(string itemName, ID id)
        {
            var database = FakeUtil.FakeDatabase();
            var item = FakeUtil.FakeItem(id, itemName, database);

            item.Should().NotBeNull();
        }

        [AutoData]
        [Theory, InlineAutoData("test item name")]
        public void FakeItem_WhenCalled_SetsItemName(string itemName, ID id)
        {
            var database = FakeUtil.FakeDatabase();
            var item = FakeUtil.FakeItem(id, itemName, database);

            item.Name.Should().Be(itemName);
        }
        
        [Theory, AutoData]
        public void FakeItem_WhenCalled_SetsItemId(string itemName, ID id)
        {
            var database = FakeUtil.FakeDatabase();
            var item = FakeUtil.FakeItem(id, itemName, database);

            item.ID.Should().Be(id);
        }

        [Theory, AutoData]
        public void FakeItem_WhenCalled_SetsItemDatabase(string itemName, ID id)
        {
            var database = FakeUtil.FakeDatabase();
            var item = FakeUtil.FakeItem(id, itemName, database);

            item.Database.Should().Be(database);
        }

        [Theory, AutoData]
        [InlineData("test item name")]
        public void FakeItem_WhenCalledWithoutId_NeverReturnsNull(string itemName)
        {
            var database = FakeUtil.FakeDatabase();

            var item = FakeUtil.FakeItem(itemName, database);

            item.Should().NotBeNull();
        }

        [Theory, AutoData]
        [InlineData("test item name")]        
        public void FakeItem_WhenCalledWithoutId_SetsName(string itemName)
        {
            var database = FakeUtil.FakeDatabase();    

            var item = FakeUtil.FakeItem(itemName, database);

            item.Name.Should().Be(itemName);
        }

        [Theory, AutoData]
        [InlineData("test item name")]
        public void FakeItem_WhenCalledWithoutId_SetsNotNullId(string itemName)
        {
            var database = FakeUtil.FakeDatabase();

            var item = FakeUtil.FakeItem(itemName, database);

            item.ID.Should().NotBe(ID.Null);
        }

        [Theory, AutoData]
        [InlineData("test item name")]
        public void FakeItem_WhenCalledWithoutId_SetsDatabase(string itemName)
        {
            var database = FakeUtil.FakeDatabase();

            var item = FakeUtil.FakeItem(itemName, database);

            item.Database.Should().Be(database);
        }

        [Fact]
        public void FakeItem_WhenCalledOnlyWithDatabase_NeverReturnsNull()
        {
            var database = FakeUtil.FakeDatabase();

            var item = FakeUtil.FakeItem(database);

            item.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCalledOnlyWithDatabase_SetsNotEmptyId()
        {
            var database = FakeUtil.FakeDatabase();

            var item = FakeUtil.FakeItem(database);

            item.ID.Should().NotBe(ID.Null);
        }

        [Fact]
        public void FakeItem_WhenCalledOnlyWithDatabase_SetsDatabase()
        {
            var database = FakeUtil.FakeDatabase();
            
            var item = FakeUtil.FakeItem(database);

            item.Database.Should().Be(database);
        }

        [Theory, InlineData("fakeItem")]        
        public void FakeItem_WhenCalledOnlyWithDatabase_SetsDefaultItemName(string defaultItemName)
        {
            var database = FakeUtil.FakeDatabase();

            var item = FakeUtil.FakeItem(database);

            item.Name.Should().Be(defaultItemName);
        }

        [Fact]
        public void FakeItem_WhenCalledWithDefaults_NeverReturnsNull()
        {
            var item = FakeUtil.FakeItem();

            item.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCalledWithDefaults_ReturnsItemWithNotNullId()
        {
            var item = FakeUtil.FakeItem();

            item.ID.Should().NotBe(ID.Null);
        }

        [Fact]
        public void FakeItem_WhenCalledWithDefaults_ReturnsItemWithNotNullDatabase()
        {
            var item = FakeUtil.FakeItem();

            item.Database.Should().NotBeNull();
        }

        [Theory, InlineData("fakeItem")]
        public void FakeItem_WhenCalledWithDefaults_SetsDefaultItemName(string defaultItemName)
        {
            var database = FakeUtil.FakeDatabase();

            var item = FakeUtil.FakeItem(database);

            item.Name.Should().Be(defaultItemName);
        }

        [Theory, AutoData]
        public void FakeField_WhenCalled_SetsFieldId(ID id)
        {            
            var item = FakeUtil.FakeItem();

            var field = FakeUtil.FakeField(id, item);

            field.ID.Should().Be(id);
        }

        [Theory, AutoData]
        public void FakeField_WhenCalled_SetsItem(ID id)
        {
            var item = FakeUtil.FakeItem();

            var field = FakeUtil.FakeField(id, item);

            field.Item.Should().Be(item);
        }

        [Theory, AutoData]
        public void FakeField_WhenCalled_SetsItemDatabase(ID id)
        {
            var item = FakeUtil.FakeItem();

            var field = FakeUtil.FakeField(id, item);

            field.Database.Should().Be(item.Database);
        }

        [Fact]
        public void FakeField_WhenCalled_SetsNotNullFieldId()
        {
            var item = FakeUtil.FakeItem();

            var field = FakeUtil.FakeField(item);

            field.ID.Should().NotBe(ID.Null);
        }

        [Fact]
        public void FakeItem_WhenCalled_DoesNotFakeFields()
        {
            var item = FakeUtil.FakeItem();

            item.Fields.Should().BeNull();
        }

        [Fact]
        public void FakeItemFields_WhenCalled_FakesItemFields()
        {
            var item = FakeUtil.FakeItem();

            FakeUtil.FakeItemFields(item);

            item.Fields
                .Should().NotBeNull()
                .And.BeAssignableTo<FieldCollection>();            
        }

        [InlineData("some field")]
        [Theory, AutoData]
        public void FakeItemFields_WhenCalled_ProducesFakeFields(string fieldName)
        {
            var item = FakeUtil.FakeItem(); 
            
            FakeUtil.FakeItemFields(item);
            var field = FakeUtil.FakeField(item);

            item.Fields[fieldName].Returns(field);
            item.Fields.Should().BeAssignableTo<FieldCollection>();
        }

        [InlineAutoData("some name", "some value")]
        [Theory, AutoData]
        public void FakeFieldValue_WhenCalled_AllowsFieldToBeFoundByID(string name, string value, ID id)
        {
            var item = FakeUtil.FakeItem();
            FakeUtil.FakeItemFields(item);

            FakeUtil.FakeFieldValue(id, name, value, item);
            
            item.Fields[id].Should().NotBeNull();
        }

        [InlineAutoData("some name", "some value")]
        [Theory, AutoData]
        public void FakeFieldValue_WhenCalled_AllowsFieldToBeFoundByName(string name, string value, ID id)
        {
            var item = FakeUtil.FakeItem();
            FakeUtil.FakeItemFields(item);

            FakeUtil.FakeFieldValue(id, name, value, item);

            item.Fields[name].Value
                .Should().NotBeNull()
                .And.Be(value);
        }

        [InlineAutoData("some name", "some value")]
        [Theory, AutoData]
        public void FakeFieldValue_WhenCalled_SpecifiedFieldWithDefaultID(string name, string value)
        {
            var item = FakeUtil.FakeItem();
            FakeUtil.FakeItemFields(item);

            FakeUtil.FakeFieldValue(name, value, item);
            
            var actualValue = item.Fields[name].Value;

            actualValue
                .Should().NotBeNull()
                .And.Be(value);
        }

        [InlineAutoData("some name", "some value")]
        [Theory, AutoData]
        public void FakeFieldValue_WhenCalledForItemWithoutFakeItemFields_ShouldThrowException(string name, string value)
        {
            // Arrange
            var item = FakeUtil.FakeItem();

            //Act
            Action attemptToFakeFieldValue = () => FakeUtil.FakeFieldValue(name, value, item);

            // Assert
            attemptToFakeFieldValue.Should().Throw<InvalidDataException>();
        }

        [Fact]
        public void FakeItem_WhenCalled_DoesNotFakePath()
        {
            var item = FakeUtil.FakeItem();
            item.Paths.Should().BeNull();
        }

        [Fact]
        public void FakeItemPaths_WhenCalled_ShouldSetItemPaths()
        {
            var item = FakeUtil.FakeItem();

            FakeUtil.FakeItemPath(item);

            item.Paths.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCalled_DoesNotFakeAccess()
        {
            var item = FakeUtil.FakeItem();

            item.Access.Should().BeNull();
        }

        [Fact]
        public void FakeItemAccess_WhenCalled_ShouldSetAccess()
        {
            var item = FakeUtil.FakeItem();

            FakeUtil.FakeItemAccess(item);

            item.Access.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCreated_DoesNotFakeUri()
        {
            var item = FakeUtil.FakeItem();
            item.Uri.Should().BeNull();
        }

        [Theory, InlineAutoData("/some item path", "some database")]
        public void FakeItemUri_WhenCalled_SetsId(string path, string databaseName, ID itemId)
        {            
            var language = Language.Invariant;
            var version = Version.Latest;

            var uri = FakeUtil.FakeItemUri(itemId, path, language, version, databaseName);

            uri.ItemID.Should().Be(itemId);
        }

        [Theory, InlineAutoData("/some item path", "some database")]
        public void FakeItemUri_WhenCalled_SetsPath(string path, string databaseName, ID itemId)
        {            
            var language = Language.Invariant;
            var version = Version.Latest;

            var uri = FakeUtil.FakeItemUri(itemId, path, language, version, databaseName);

            uri.Path.Should().Be(path);
        }

        [Theory, InlineAutoData("/some item path", "some database")]
        public void FakeItemUri_WhenCalled_SetsDatabaseName(string path, string databaseName, ID itemId)
        {           
            var language = Language.Invariant;
            var version = Version.Latest;

            var uri = FakeUtil.FakeItemUri(itemId, path, language, version, databaseName);

            uri.DatabaseName.Should().Be(databaseName);
        }

        [Theory, InlineAutoData("/some item path", "some database")]
        public void FakeItemUri_WhenCalled_SetsLanguage(string path, string databaseName, ID itemId)
        {
            var language = Language.Invariant;
            var version = Version.Latest;

            var uri = FakeUtil.FakeItemUri(itemId, path, language, version, databaseName);

            uri.Language.Should().Be(language);
        }

        [Theory, InlineAutoData("/some item path", "some database")]
        public void FakeItemUri_WhenCalled_SetsVersion(string path, string databaseName, ID itemId)
        {
            var language = Language.Invariant;
            var version = Version.Latest;

            var uri = FakeUtil.FakeItemUri(itemId, path, language, version, databaseName);

            uri.Version.Should().Be(version);
        }

        [Fact]
        public void FakeItemUri_WhenCalledWithoutArgs_NeverReturnsNull()
        {
            var uri = FakeUtil.FakeItemUri();

            uri.Should().NotBeNull();
        }

        [Fact]
        public void FakeItemUri_WhenCalledWithoutArgs_SetsNonEmptyItemId()
        {
            var uri = FakeUtil.FakeItemUri();     
            
            uri.ItemID.Should().NotBe(ID.Null);
        }

        [Fact]
        public void FakeItemUri_WhenCalledWithoutArgs_SetsInvariantLanguage()
        {
            var uri = FakeUtil.FakeItemUri();

            uri.Language.Should().Be(Language.Invariant);
        }

        [Fact]
        public void FakeItemUri_WhenCalledWithoutArgs_SetsEmptyPath()
        {
            var uri = FakeUtil.FakeItemUri();

            uri.Path.Should().Be(string.Empty);
        }

        [Fact]
        public void FakeItemUri_WhenCalledWithoutArgs_SetsVersionToLatest()
        {
            var uri = FakeUtil.FakeItemUri();

            uri.Version.Should().Be(Version.Latest);
        }

        [Fact]
        public void FakeItemUri_WhenCalledWithoutArgs_SetsDatabaseNameToDefaultValue()
        {
            var uri = FakeUtil.FakeItemUri();

            uri.DatabaseName.Should().Be("fakeDatabase");
        }

        [Theory, AutoData]
        public void FakeItemUri_WhenCalledWithItem_NeverReturnsNull(string itemName, ID itemId, int versionNumber, string databaseName)
        {
            var database = FakeUtil.FakeDatabase(databaseName);
            var item = FakeUtil.FakeItem(itemId, itemName, database);
            FakeUtil.FakeItemLanguage(item);

            var version = Version.Parse(versionNumber);
            item.Version.Returns(version);

            var uri = FakeUtil.FakeItemUri(item).Uri;

            uri.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void FakeItemUri_WhenCalledWithItem_SetsItemId(string itemName, ID itemId, int versionNumber, string databaseName)
        {
            var database = FakeUtil.FakeDatabase(databaseName);
            var item = FakeUtil.FakeItem(itemId, itemName, database);
            FakeUtil.FakeItemLanguage(item);

            var version = Version.Parse(versionNumber);
            item.Version.Returns(version);

            var uri = FakeUtil.FakeItemUri(item).Uri;

            uri.ItemID.Should().Be(itemId);
        }

        [Theory, AutoData]
        public void FakeItemUri_WhenCalledWithItem_SetsLanguage(string itemName, ID itemId, int versionNumber, string databaseName)
        {
            var database = FakeUtil.FakeDatabase(databaseName);
            var item = FakeUtil.FakeItem(itemId, itemName, database);
            FakeUtil.FakeItemLanguage(item);

            var version = Version.Parse(versionNumber);
            item.Version.Returns(version);

            var uri = FakeUtil.FakeItemUri(item).Uri;

            uri.Language.Should().Be(item.Language);
        }

        [Theory, AutoData]
        public void FakeItemUri_WhenCalledWithItem_SetsVersion(string itemName, ID itemId, int versionNumber, string databaseName)
        {
            var database = FakeUtil.FakeDatabase(databaseName);
            var item = FakeUtil.FakeItem(itemId, itemName, database);
            FakeUtil.FakeItemLanguage(item);

            var version = Version.Parse(versionNumber);
            item.Version.Returns(version);

            var uri = FakeUtil.FakeItemUri(item).Uri;

            uri.Version.Should().Be(version);
        }

        [Theory, AutoData]
        public void FakeItemUri_WhenCalledWithItem_SetsDatabaseName(string itemName, ID itemId, int versionNumber, string databaseName)
        {
            var database = FakeUtil.FakeDatabase(databaseName);
            var item = FakeUtil.FakeItem(itemId, itemName, database);
            FakeUtil.FakeItemLanguage(item);

            var version = Version.Parse(versionNumber);
            item.Version.Returns(version);

            var uri = FakeUtil.FakeItemUri(item).Uri;

            uri.DatabaseName.Should().Be(databaseName);
        }

        [Theory, AutoData]
        public void FakeDataUri_WhenCalledWithParams_NeverReturnsNull(ID id, int versionNumber)
        {            
            var language = Language.Invariant;
            var version = Version.Parse(versionNumber);

            var uri = FakeUtil.FakeDataUri(id, language, version);

            uri.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void FakeDataUri_WhenCalledWithParams_SetsItemId(ID id, int versionNumber)
        {
            var language = Language.Invariant;
            var version = Version.Parse(versionNumber);

            var uri = FakeUtil.FakeDataUri(id, language, version);

            uri.ItemID.Should().Be(id);
        }

        [Theory, AutoData]
        public void FakeDataUri_WhenCalledWithParams_SetsVersion(ID id, int versionNumber)
        {
            var language = Language.Invariant;
            var version = Version.Parse(versionNumber);

            var uri = FakeUtil.FakeDataUri(id, language, version);

            uri.Version.Should().Be(version);
        }

        [Theory, AutoData]
        public void FakeDataUri_WhenCalledWithParams_SetsLanguage(ID id, int versionNumber)
        {
            var language = Language.Invariant;
            var version = Version.Parse(versionNumber);

            var uri = FakeUtil.FakeDataUri(id, language, version);

            uri.Language.Should().Be(language);
        }

        [Fact]
        public void FakeDataUri_WhenCalledWithoutParams_NeverReturnsNull()
        {
            var uri = FakeUtil.FakeDataUri();

            uri.Should().NotBeNull();
        }

        [Fact]
        public void FakeDataUri_WhenCalledWithoutParams_ShouldSetNotNullId()
        {
            var uri = FakeUtil.FakeDataUri();

            uri.ItemID.Should().NotBe(ID.Null);
        }

        [Fact]
        public void FakeDataUri_WhenCalledWithoutParams_ShouldSetInvariantLanguage()
        {
            var uri = FakeUtil.FakeDataUri();

            uri.Language.Should().Be(Language.Invariant);
        }

        [Fact]
        public void FakeDataUri_WhenCalledWithoutParams_ShouldSetToLatestVersion()
        {
            var uri = FakeUtil.FakeDataUri();

            uri.Should().NotBeNull();

            uri.Version.Should().Be(Version.Latest);
        }

        [Fact]
        public void FakeItem_WhenCreated_DoesNotSetAppearance()
        {
            var item = FakeUtil.FakeItem();
            item.Appearance.Should().BeNull();
        }

        [Fact]
        public void FakeItemAppearance_WhenCalled_SetsAppearance()
        {
            var item = FakeUtil.FakeItem();
            
            FakeUtil.FakeItemAppearance(item);

            item.Appearance.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCreated_DoesNotSetStatistics()
        {
            var item = FakeUtil.FakeItem();
            item.Statistics.Should().BeNull();
        }

        [Fact]
        public void FakeItemStatistics_WhenCalled_SetsStatistics()
        {
            var item = FakeUtil.FakeItem();            

            FakeUtil.FakeItemStatistics(item);

            item.Statistics.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCreated_DoesNotSetItemTemplate()
        {
            var item = FakeUtil.FakeItem();
            item.Template.Should().BeNull();
        }

        [Fact]
        public void FakeItemTemplate_WhenCalled_SetsItemTemplate()
        {
            var item = FakeUtil.FakeItem();

            FakeUtil.FakeItemTemplate(item);

            item.Template.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCreated_DoesNotSetItemLinks()
        {
            var item = FakeUtil.FakeItem();
            item.Links.Should().BeNull();
        }

        [Fact]
        public void FakeItemLinks_WhenCalled_SetsItemLinks()
        {
            var item = FakeUtil.FakeItem();

            FakeUtil.FakeItemLinks(item);

            item.Links.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCreated_DoesNotSetItemLocking()
        {
            var item = FakeUtil.FakeItem();
            item.Locking.Should().BeNull();
        }

        [Fact]
        public void FakeItemLocking_WhenCalled_SetsItemLocking()
        {
            var item = FakeUtil.FakeItem();            

            FakeUtil.FakeItemLocking(item);

            item.Locking.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCreated_DoesNotSetVersions()
        {
            var item = FakeUtil.FakeItem();
            item.Versions.Should().BeNull();
        }

        [Fact]
        public void FakeItemVersions_WhenCalled_SetsItemVersions()
        {
            var item = FakeUtil.FakeItem();

            FakeUtil.FakeItemVersions(item);

            item.Versions.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCreated_DoesNotSetAxes()
        {
            var item = FakeUtil.FakeItem();
            item.Axes.Should().BeNull();
        }

        [Fact]
        public void FakeItemAxes_WhenCalled_SetsItemAxes()
        {
            var item = FakeUtil.FakeItem();

            FakeUtil.FakeItemAxes(item);

            item.Axes.Should().NotBeNull();
        }

        [Fact]
        public void FakeItem_WhenCreated_DoesNotSetEditing()
        {
            var item = FakeUtil.FakeItem();
            item.Editing.Should().BeNull();
        }

        [Fact]
        public void FakeItemEditing_WhenCalled_SetsItemEditing()
        {
            var item = FakeUtil.FakeItem();
            item.Editing.Should().BeNull();

            FakeUtil.FakeItemEditing(item);

            item.Editing.Should().NotBeNull();
        }
    }
}
