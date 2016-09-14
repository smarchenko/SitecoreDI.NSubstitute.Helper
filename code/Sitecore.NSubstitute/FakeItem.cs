using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Engines;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Templates;

namespace Sitecore.NSubstitute
{
  public class FakeItem : IEnumerable
  {
    private readonly ItemList childList = new ItemList();

    public FakeItem(ID id = null, Database database = null)
    {
      this.Item = FakeUtil.FakeItem(id ?? ID.NewID, "fakeItem", database ?? FakeUtil.FakeDatabase());
      FakeUtil.FakeItemFields(this.Item);
      FakeUtil.FakeItemEditing(this.Item);
      
      var templateItem = Substitute.For<TemplateItem>(this.Item);
      this.Item.Template.Returns(templateItem);

      this.Item.Children.Returns(new ChildList(this.Item, this.childList));
      this.Item.Database.GetItem(this.Item.ID).Returns(this.Item);
      this.Item.Database.GetItem(this.Item.ID.ToString()).Returns(this.Item);
    }

    public ID ID
    {
      get
      {
        return this.Item.ID;
      }
    }

    private Item Item { get; set; }

    public static implicit operator Item(FakeItem fakeItem)
    {
      return fakeItem.Item;
    }

    public IEnumerator GetEnumerator()
    {
      throw new NotImplementedException();
    }

    public void Add(ID id, string name, string value)
    {
      this.WithField(id, name, value);
    }

    public void Add(string name, string value)
    {
      this.WithField(name, value);
    }

    public void Add(ID id, string value)
    {
      this.WithField(id, value);
    }

    public FakeItem WithTemplate(ID templateId)
    {
      this.Item.Template.ID.Returns(templateId);

      this.Item.TemplateID.Returns(templateId);

      var runtimeSettings = Substitute.For<ItemRuntimeSettings>(this.Item);
      runtimeSettings.TemplateDatabase.Returns(this.Item.Database);
      this.Item.RuntimeSettings.Returns(runtimeSettings);


      var engines = Substitute.For<DatabaseEngines>(this.Item.Database);
      var templateEngine = Substitute.For<TemplateEngine>(this.Item.Database);
      var template = new Template.Builder(templateId.ToString(), templateId, new TemplateCollection());

      templateEngine.GetTemplate(templateId).Returns(template.Template);

      engines.TemplateEngine.Returns(templateEngine);
      this.Item.Database.Engines.Returns(engines);
      this.Item.Database.GetTemplate(templateId).Returns(this.Item.Template);

      return this;
    }

    public FakeItem WithName(string name)
    {
      this.Item.Name.Returns(name);
      return this;
    }

    public FakeItem WithChild(FakeItem child)
    {
      this.childList.Add(child);

      return this;
    }

    public FakeItem WithParent(FakeItem parent)
    {
      parent.WithChild(this);
      this.Item.Parent.Returns(parent);
      this.Item.ParentID.Returns(parent.ID);
      return this;
    }

    public FakeItem WithField(string name, string value)
    {
      return this.WithField(ID.NewID, name, value);
    }

    public FakeItem WithField(ID id, string value)
    {
      return this.WithField(id, string.Empty, value);
    }

    public FakeItem WithField(ID id, string name, string value)
    {
      var field = Substitute.For<Field>(id, this.Item);
      field.Name.Returns(name);
      field.Value.Returns(value);

      this.Item.Fields[name].Returns(field);
      this.Item.Fields[id].Returns(field);

      this.Item[id].Returns(value);
      this.Item[name].Returns(value);

      var sectionItem = Substitute.For<TemplateSectionItem>(this.Item, this.Item.Template);
      var templateField = Substitute.For<TemplateFieldItem>(this.Item, sectionItem);

      this.Item.Template.GetField(name).Returns(templateField);
      this.Item.Template.GetField(id).Returns(templateField);

      return this;
    }

    public void Add(FakeItem child)
    {
      this.WithChild(child);
    }
  }
}
