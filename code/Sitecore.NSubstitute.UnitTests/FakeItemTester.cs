using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Data.Locking;

namespace Sitecore.NSubstitute.UnitTests
{
  [TestFixture]
  public class FakeItemTester
  {
    [Test]
    public void FakeItem_CanBeInstantiated()
    {
      var item = new FakeItem();
    }

    [Test]
    public void FakeItem_ShouldInitializeProperties()
    {
      var fakeItem = new FakeItem();
      fakeItem.ID.Should().NotBeNull();
      fakeItem.ID.Should().NotBe(ID.Null);

      var item = (Item)fakeItem;
      item.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldInitialize_ItemProperties()
    {
      var item = (Item) new FakeItem();

      item.Fields.Should().NotBeNull();
      item.Template.Should().NotBeNull();
      item.Children.Should().NotBeNull();
      item.Children.Count.Should().Be(0);
      item.Database.Should().NotBeNull();
      item.Database.GetItem(item.ID).Should().Be(item);
      item.Language.Should().Be(Globalization.Language.Invariant);
      item.Version.Should().Be(Sitecore.Data.Version.First);
    }

    [Test]
    public void FakeItem_ShouldInitializeField_ByNameIDValue()
    {
      var item = new FakeItem();
      ID id = ID.NewID;
      string name = "test field";
      string value = "test value";

      item.Add(id, name, value);

      var scItem = (Item) item;

      scItem.Fields[id].Should().NotBeNull();
      scItem.Fields[name].Should().NotBeNull();
      scItem.Fields[id].Value.Should().Be(value);
      scItem[id].Should().Be(value);
      scItem[name].Should().Be(value);
    }

    [Test]
    public void FakeItem_ShouldInitializeField_ByIDValue()
    {
      var item = new FakeItem();
      ID id = ID.NewID;
      string value = "test value";

      item.Add(id, value);

      var scItem = (Item)item;

      scItem.Fields[id].Should().NotBeNull();
      scItem.Fields[id].Name.Should().Be(string.Empty);
      scItem.Fields[id].Value.Should().Be(value);
      scItem[id].Should().Be(value);
    }

    [Test]
    public void FakeItem_ShouldInitializeField_ByNameValue()
    {
      var item = new FakeItem();
      string name = "test field";
      string value = "test value";

      item.Add(name, value);

      var scItem = (Item)item;

      scItem.Fields[name].Should().NotBeNull();
      scItem.Fields[name].ID.Should().NotBe(ID.Null);
      scItem.Fields[name].ID.Should().NotBeNull();
      scItem[name].Should().Be(value);
    }

    [Test]
    public void FakeItem_ShouldSimplify_StructureCreation()
    {
      var item = new FakeItem();

      var parentID = ID.NewID;
      var childID1 = ID.NewID;
      var childID2 = ID.NewID;
      var scItem = (Item)item;
      item
        .WithParent(new FakeItem(parentID))
        .WithChild(new FakeItem(childID1, scItem.Database))
        .WithChild(new FakeItem(childID2, scItem.Database));

      scItem.ParentID.Should().Be(parentID);
      scItem.Parent.ID.Should().Be(parentID);
      scItem.Children.Count.Should().Be(2);

      scItem.Database.GetItem(childID1).Should().NotBeNull();
      scItem.Database.GetItem(childID1).ID.Should().Be(childID1);
    }

    [Test]
    public void FakeItem_ShouldInitialize_Template()
    {
      var templateId = ID.NewID;
      var item = new FakeItem().WithTemplate(templateId);

      var scItem = (Item) item;
      scItem.Template.Should().NotBeNull();
      scItem.TemplateID.Should().Be(templateId);
      scItem.Template.ID.Should().Be(templateId);
      scItem.Database.GetTemplate(templateId).Should().NotBeNull();
      scItem.Database.Engines.TemplateEngine.GetTemplate(templateId).Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldSet_ItemName()
    {
      var name = "my test item";
      var item = new FakeItem()
                          .WithName(name)
                          .ToSitecoreItem();

      item.Name.Should().Be(name);
    }

    [Test]
    public void FakeItem_ShouldAdd_ChildItem()
    {
      var item = new FakeItem();
      item.Add(new FakeItem());

      item.ToSitecoreItem().Children.Count.Should().Be(1);
    }

    [Test]
    public void FakeItem_ShouldInitialize_Paths()
    {
      var item = new FakeItem().ToSitecoreItem();

      item.Paths.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldFake_ItemPath()
    {
      string itemPath = "/test/somepath";
      var item = new FakeItem().WithPath(itemPath).ToSitecoreItem();

      item.Paths.FullPath.Should().Be(itemPath);
      item.Database.GetItem(itemPath).Should().Be(item);
    }

    [Test]
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

    [Test]
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

    [Test]
    public void FakeItem_ShouldFake_LanguageByName()
    {
      var fakeItem = new FakeItem()
                      .WithLanguage("en-US");

      var item = fakeItem.ToSitecoreItem();
      item.Language.Should().NotBeNull();
      item.Language.Name.Should().Be("en-US");
      item.Database.GetItem(item.ID, item.Language).Should().Be(item);
    }

    [Test]
    public void FakeItem_ShouldFake_LanguageObject()
    {
      var fakeItem = new FakeItem()
                      .WithLanguage(Sitecore.Globalization.Language.Parse("en-US"));

      var item = fakeItem.ToSitecoreItem();
      item.Language.Should().NotBeNull();
      item.Language.Name.Should().Be("en-US");
      item.Database.GetItem(item.ID, item.Language).Should().Be(item);
    }

    [Test]
    public void FakeItem_ShouldFake_Uri()
    {
      var fakeItem = new FakeItem();
      fakeItem
        .WithPath("/test/path")
        .WithLanguage("da")
        .WithVersion(4)
        .WithUri();

      var item = fakeItem.ToSitecoreItem();
      item.Uri.Should().NotBeNull();
      item.Uri.ItemID.Should().Be(item.ID);
      item.Uri.Path.Should().Be(item.Paths.FullPath);
      item.Uri.DatabaseName.Should().Be(item.Database.Name);
      item.Uri.Language.Should().Be(item.Language);
      item.Uri.Version.Should().Be(item.Version);
      item.Database.GetItem(item.Uri.ToDataUri()).Should().Be(item);
    }

    [Test]
    public void FakeItem_ShouldFake_Appearance()
    {
      var item = new FakeItem();

      item.ToSitecoreItem().Appearance.Should().BeNull();
      item.WithAppearance(Substitute.For<ItemAppearance>(item.ToSitecoreItem()));

      item.ToSitecoreItem().Appearance.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldFake_Statistics()
    {
      var item = new FakeItem();

      item.ToSitecoreItem().Statistics.Should().BeNull();
      item.WithStatistics(Substitute.For<ItemStatistics>(item.ToSitecoreItem()));

      item.ToSitecoreItem().Statistics.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldFake_Links()
    {
      var item = new FakeItem();

      item.ToSitecoreItem().Links.Should().BeNull();
      item.WithItemLinks(Substitute.For<ItemLinks>(item.ToSitecoreItem()));

      item.ToSitecoreItem().Links.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldFake_Locking()
    {
      var item = new FakeItem();

      item.ToSitecoreItem().Locking.Should().BeNull();
      item.WithItemLocking(Substitute.For<ItemLocking>(item.ToSitecoreItem()));

      item.ToSitecoreItem().Locking.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldFake_Versions()
    {
      var item = new FakeItem();

      item.ToSitecoreItem().Versions.Should().BeNull();
      item.WithItemVersions(Substitute.For<ItemVersions>(item.ToSitecoreItem()));

      item.ToSitecoreItem().Versions.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldFake_Axes()
    {
      var item = new FakeItem();

      item.ToSitecoreItem().Axes.Should().BeNull();
      item.WithItemAxes(Substitute.For<ItemAxes>(item.ToSitecoreItem()));

      item.ToSitecoreItem().Axes.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldFake_ItemEditing()
    {
      var item = new FakeItem();

      item.ToSitecoreItem().Editing.Should().BeNull();
      item.WithItemEditing(Substitute.For<ItemEditing>(item.ToSitecoreItem()));

      item.ToSitecoreItem().Editing.Should().NotBeNull();
    }

    [Test]
    public void FakeItem_ShouldSupportExtanding_WithExtensionMethods()
    {
      var item = new FakeItem();

      item.ToSitecoreItem().Help.Should().BeNull();

      item.WithItemHelp(Substitute.For<ItemHelp>(item.ToSitecoreItem()));

      item.ToSitecoreItem().Help.Should().NotBeNull();
    }
  }

  public static class TestFakeItemExtensions
  {
    public static FakeItem WithItemHelp(this FakeItem item, ItemHelp itemHelp)
    {
      item.ToSitecoreItem().Help.Returns(itemHelp);

      return item;
    }
  }
}
