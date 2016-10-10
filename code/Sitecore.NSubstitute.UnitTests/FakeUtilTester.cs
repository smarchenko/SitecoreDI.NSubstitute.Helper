using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Globalization;

namespace Sitecore.NSubstituteUtils.UnitTests
{
  [TestFixture]
  public class FakeUtilTester
  {
    [Test]
    public void FakeDatabase_ShouldReturn_FakeDatabaseWithSpecifiedName()
    {
      string databaseName = "fake name";
      var database = FakeUtil.FakeDatabase(databaseName);

      database.Should().NotBeNull();
      database.Should().BeAssignableTo<Database>();
      database.Name.Should().Be(databaseName);
    }

    [Test]
    public void FakeDatabase_ShouldReturn_FakeDatabaseWithDefaultName()
    {
      var database = FakeUtil.FakeDatabase();

      database.Should().NotBeNull();
      database.Should().BeAssignableTo<Database>();
      database.Name.Should().Be("fakeDB");
    }

    [Test]
    public void FakeItem_ShouldReturn_FakeItemWithSpecifiedParameters()
    {
      var database = FakeUtil.FakeDatabase();
      var id = ID.NewID;
      var name = "test item name";

      var item = FakeUtil.FakeItem(id, name, database);

      item.Should().NotBeNull();
      item.ID.Should().Be(id);
      item.Name.Should().Be(name);
      item.Database.Should().Be(database);
    }

    [Test]
    public void FakeItem_ShouldReturn_FakeItemWithSpecifiedNameAndDatabase()
    {
      var database = FakeUtil.FakeDatabase();
      var name = "test item name";

      var item = FakeUtil.FakeItem(name, database);

      item.Should().NotBeNull();
      item.ID.Should().NotBe(ID.Null);
      item.Name.Should().Be(name);
      item.Database.Should().Be(database);
    }

    [Test]
    public void FakeItem_ShouldReturn_FakeItemWithDefaultNameAndDatabase()
    {
      var database = FakeUtil.FakeDatabase();
      var name = "fakeItem";

      var item = FakeUtil.FakeItem(database);

      item.Should().NotBeNull();
      item.ID.Should().NotBe(ID.Null);
      item.Name.Should().Be(name);
      item.Database.Should().Be(database);
    }

    [Test]
    public void FakeItem_ShouldReturn_FakeItemWithSpecifiedName()
    {
      var name = "test fake item";

      var item = FakeUtil.FakeItem(name);

      item.Should().NotBeNull();
      item.ID.Should().NotBe(ID.Null);
      item.Name.Should().Be(name);
      item.Database.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldReturn_FakeItemWithDefaultValues()
    {
      var name = "fakeItem";

      var item = FakeUtil.FakeItem();

      item.Should().NotBeNull();
      item.ID.Should().NotBe(ID.Null);
      item.Name.Should().Be(name);
      item.Database.Should().NotBeNull();
    }

    [Test]
    public void FakeField_ShouldReturn_FakeFieldWithSpecidiedParameters()
    {
      var id = ID.NewID;
      var item = FakeUtil.FakeItem();

      var field = FakeUtil.FakeField(id, item);

      field.Item.Should().Be(item);
      field.ID.Should().Be(id);
      field.Database.Should().Be(item.Database);
    }

    [Test]
    public void FakeField_ShouldReturn_FakeFieldWithDefaultID()
    {
      var item = FakeUtil.FakeItem();

      var field = FakeUtil.FakeField(item);

      field.Item.Should().Be(item);
      field.ID.Should().NotBe(ID.Null);
      field.Database.Should().Be(item.Database);
    }

    [Test]
    public void FakeItemFields_ShouldReturn_FakeCollection()
    {
      var item = FakeUtil.FakeItem();
      item.Fields.Should().BeNull();

      FakeUtil.FakeItemFields(item);

      item.Fields.Should().NotBeNull();
      item.Fields.Should().BeAssignableTo<FieldCollection>();
    }

    [Test]
    public void FakeItemFields_ShouldReturn_AllowsToFakeField()
    { 
      var item = FakeUtil.FakeItem();
      item.Fields.Should().BeNull();

      FakeUtil.FakeItemFields(item);

      item.Fields.Should().NotBeNull();
      var field = FakeUtil.FakeField(item);
      item.Fields["some field"].Returns(field);
      item.Fields.Should().BeAssignableTo<FieldCollection>();
    }

    [Test]
    public void FakeFieldValue_ShouldFake_SpecifiedField()
    {
      var id = ID.NewID;
      var name = "some name";
      var value = "some value";
      var item = FakeUtil.FakeItem();
      FakeUtil.FakeItemFields(item);

      FakeUtil.FakeFieldValue(id, name, value, item);

      item.Fields[name].Should().NotBeNull();
      item.Fields[id].Should().NotBeNull();
      item.Fields[name].Value.Should().Be(value);
    }

    [Test]
    public void FakeFieldValue_ShouldFake_SpecifiedFieldWithDefaultID()
    {
      var name = "some name";
      var value = "some value";
      var item = FakeUtil.FakeItem();
      FakeUtil.FakeItemFields(item);

      FakeUtil.FakeFieldValue(name, value, item);

      item.Fields[name].Should().NotBeNull();
      item.Fields[name].Value.Should().Be(value);
      item.Fields[name].ID.Should().NotBe(ID.Null);
    }

    [Test]
    public void FakeFieldValue_ShouldThrow_IfFieldsAreNotFaked()
    {
      var item = FakeUtil.FakeItem();
      
      Assert.Catch<InvalidDataException>(delegate { FakeUtil.FakeFieldValue("test name", "test value", item); });
    }

    [Test]
    public void FakeItemPaths_ShouldFake_ItemPaths()
    {
      var item = FakeUtil.FakeItem();
      item.Paths.Should().BeNull();

      FakeUtil.FakeItemPath(item);

      item.Paths.Should().NotBeNull();
    }

    [Test]
    public void FakeItemAccess_ShouldFakeItemAccess()
    {
      var item = FakeUtil.FakeItem();
      item.Access.Should().BeNull();

      FakeUtil.FakeItemAccess(item);

      item.Access.Should().NotBeNull();
    }

    [Test]
    public void FakeItemUri_ShouldFake_ItemUriWithSpecifiedParameters()
    {
      var item = FakeUtil.FakeItem();
      item.Uri.Should().BeNull();

      var id = ID.NewID;
      var path = "/some item path";
      var language = Language.Invariant;
      var version = Sitecore.Data.Version.Latest;
      var databaseName = "some database";
      var uri = FakeUtil.FakeItemUri(id, path, language, version, databaseName);

      uri.Should().NotBeNull();
      uri.ItemID.Should().Be(id);
      uri.Language.Should().Be(language);
      uri.Path.Should().Be(path);
      uri.Version.Should().Be(version);
      uri.DatabaseName.Should().Be(databaseName);
    }

    [Test]
    public void FakeItemUri_ShouldFake_ItemUriWithDefaultValues()
    {
      var uri = FakeUtil.FakeItemUri();
      uri.Should().NotBeNull();
      uri.ItemID.Should().NotBe(ID.Null);
      uri.Language.Should().Be(Language.Invariant);
      uri.Path.Should().Be(string.Empty);
      uri.Version.Should().Be(Sitecore.Data.Version.Latest);
      uri.DatabaseName.Should().Be("fakeDatabase");
    }

    [Test]
    public void FakeItemUri_ShouldFake_ItemUriWithDateTakenFromItem()
    {
      var item = FakeUtil.FakeItem();
      FakeUtil.FakeItemLanguage(item);
      item.Version.Returns(Substitute.For<Sitecore.Data.Version>(5));

      var uri = FakeUtil.FakeItemUri(item).Uri;
      
      uri.Should().NotBeNull();
      uri.ItemID.Should().Be(item.ID);
      uri.Language.Should().Be(item.Language);
      uri.Version.Should().Be(item.Version);
      uri.DatabaseName.Should().Be(item.Database.Name);
    }

    [Test]
    public void FakeDataUri_ShouldFake_DataUriWithSpecifiedParameters()
    {
      var id = ID.NewID;
      var language = Language.Invariant;
      var version = Sitecore.Data.Version.Latest;
      var uri = FakeUtil.FakeDataUri(id, language, version);

      uri.Should().NotBeNull();
      uri.ItemID.Should().Be(id);
      uri.Language.Should().Be(language);
      uri.Version.Should().Be(version);
    }

    [Test]
    public void FakeDataUri_ShouldFake_DataUriWithDefaultParameters()
    {
      var uri = FakeUtil.FakeDataUri();

      uri.Should().NotBeNull();
      uri.ItemID.Should().NotBe(ID.Null);
      uri.Language.Should().Be(Language.Invariant);
      uri.Version.Should().Be(Sitecore.Data.Version.Latest);
    }

    [Test]
    public void FakeItemAppearance_ShouldFake_Appearance()
    {
      var item = FakeUtil.FakeItem();
      item.Appearance.Should().BeNull();

      FakeUtil.FakeItemAppearance(item);

      item.Appearance.Should().NotBeNull();
    }

    [Test]
    public void FakeItemStatistics_ShouldFake_Statistics()
    {
      var item = FakeUtil.FakeItem();
      item.Statistics.Should().BeNull();

      FakeUtil.FakeItemStatistics(item);

      item.Statistics.Should().NotBeNull();
    }

    [Test]
    public void FakeItemTemplate_ShouldFake_ItemTemplate()
    {
      var item = FakeUtil.FakeItem();
      item.Template.Should().BeNull();

      FakeUtil.FakeItemTemplate(item);

      item.Template.Should().NotBeNull();
    }

    [Test]
    public void FakeItemLinks_ShouldFake_ItemLinks()
    {
      var item = FakeUtil.FakeItem();
      item.Links.Should().BeNull();

      FakeUtil.FakeItemLinks(item);

      item.Links.Should().NotBeNull();
    }

    [Test]
    public void FakeItemLocking_ShouldFake_ItemLocking()
    {
      var item = FakeUtil.FakeItem();
      item.Locking.Should().BeNull();

      FakeUtil.FakeItemLocking(item);

      item.Locking.Should().NotBeNull();
    }

    [Test]
    public void FakeItemVersions_ShouldFake_ItemVersions()
    {
      var item = FakeUtil.FakeItem();
      item.Versions.Should().BeNull();

      FakeUtil.FakeItemVersions(item);

      item.Versions.Should().NotBeNull();
    }

    [Test]
    public void FakeItemAxes_ShouldFake_ItemAxes()
    {
      var item = FakeUtil.FakeItem();
      item.Axes.Should().BeNull();

      FakeUtil.FakeItemAxes(item);

      item.Axes.Should().NotBeNull();
    }

    [Test]
    public void FakeItemEditing_ShouldFake_ItemEditing()
    {
      var item = FakeUtil.FakeItem();
      item.Editing.Should().BeNull();

      FakeUtil.FakeItemEditing(item);

      item.Editing.Should().NotBeNull();
    }
  }
}
