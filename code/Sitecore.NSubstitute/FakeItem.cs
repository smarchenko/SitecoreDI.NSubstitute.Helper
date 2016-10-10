using NSubstitute;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Engines;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Locking;
using Sitecore.Data.Templates;
using Sitecore.Links;
using Sitecore.Security.AccessControl;

namespace Sitecore.NSubstituteUtils
{
  public class FakeItem
  {
    private readonly ItemList childList = new ItemList();

    public FakeItem(ID id = null, Database database = null)
    {
      this.Item = FakeUtil.FakeItem(id ?? ID.NewID, "fakeItem", database ?? FakeUtil.FakeDatabase());
      FakeUtil.FakeItemFields(this.Item);
      FakeUtil.FakeItemPath(this.Item);
      
      var templateItem = Substitute.For<TemplateItem>(this.Item);
      this.Item.Template.Returns(templateItem);
      Item.Language.Returns(Globalization.Language.Invariant);
      Item.Version.Returns(Version.First);

      this.Item.Children.Returns(new ChildList(this.Item, this.childList));
      this.Item.Database.GetItem(this.Item.ID).Returns(this.Item);
      this.Item.Database.GetItem(this.Item.ID.ToString()).Returns(this.Item);
      this.Item.Database.GetItem(this.Item.ID, Item.Language, Item.Version).Returns(this.Item);
      this.Item.Database.GetItem(this.Item.ID, Item.Language).Returns(this.Item);
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

    /// <summary>
    /// Converts current instance to Sitecore item.
    /// </summary>
    /// <returns></returns>
    public Item ToSitecoreItem()
    {
      return this.Item;
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
      this.Item.Database.GetTemplate(templateId).Returns(d => this.Item.Template);

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

    public FakeItem WithPath(string itemPath)
    {
      this.Item.Paths.FullPath.Returns(itemPath);
      this.Item.Database.GetItem(itemPath).Returns(this.Item);
      return this;
    }

    public FakeItem WithItemAccess()
    {
      FakeUtil.FakeItemAccess(this.Item);
      return this;
    }

    public FakeItem WithUri()
    {
      var uri = Substitute.For<ItemUri>(Item.ID, Item.Paths == null? string.Empty : Item.Paths.FullPath, Item.Language ?? Sitecore.Globalization.Language.Invariant, Item.Version ?? Version.Latest, Item.Database.Name);
      return WithUri(uri);
    }

    public FakeItem WithUri(ItemUri uri)
    {
      Item.Uri.Returns(uri);
      Item.Database.GetItem(this.Item.Uri.ToDataUri()).Returns(Item);
      return this;
    }

    public FakeItem WithLanguage(string languageName)
    {
      return WithLanguage(Globalization.Language.Parse(languageName));
    }

    public FakeItem WithLanguage(Sitecore.Globalization.Language language)
    {
      Item.Language.Returns(language);

      Item.Database.GetItem(Item.ID, Item.Language).Returns(Item);
      Item.Database.GetItem(Item.ID, Item.Language, Item.Version ?? Version.First).Returns(Item);
      return this;
    }

    public FakeItem WithVersion(int number)
    {
      var version = Version.Parse(number);

      return WithVersion(version);
    }

    public FakeItem WithVersion(Sitecore.Data.Version version)
    {
      Item.Version.Returns(version);

      Item.Database.GetItem(Item.ID, Item.Language ?? Sitecore.Globalization.Language.Invariant, Item.Version).Returns(Item);
      return this;
    }

    public FakeItem WithAppearance(ItemAppearance appearance)
    {
      Item.Appearance.Returns(appearance);

      return this;
    }

    public FakeItem WithStatistics(ItemStatistics statistics)
    {
      Item.Statistics.Returns(statistics);

      return this;
    }

    public FakeItem WithItemLinks(ItemLinks links)
    {
      Item.Links.Returns(links);

      return this;
    }

    public FakeItem WithItemLocking(ItemLocking itemLocking)
    {
      Item.Locking.Returns(itemLocking);

      return this;
    }

    public FakeItem WithItemVersions(ItemVersions versions)
    {
      Item.Versions.Returns(versions);

      return this;
    }

    public FakeItem WithItemAxes(ItemAxes axes)
    {
      Item.Axes.Returns(axes);

      return this;
    }

    public FakeItem WithItemEditing(ItemEditing itemEditing)
    {
      Item.Editing.Returns(itemEditing);

      return this;
    }
  }
}
