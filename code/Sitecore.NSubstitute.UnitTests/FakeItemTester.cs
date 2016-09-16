using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;

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
      item.Editing.Should().NotBeNull();
      item.Template.Should().NotBeNull();
      item.Children.Should().NotBeNull();
      item.Children.Count.Should().Be(0);
      item.Database.Should().NotBeNull();
      item.Database.GetItem(item.ID).Should().Be(item);
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
  }
}
