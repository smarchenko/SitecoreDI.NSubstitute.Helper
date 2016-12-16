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
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using NSubstitute.Core;

  using Sitecore.Globalization;

  using Version = Sitecore.Data.Version;

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
      this.Item.HasChildren.Returns(o => this.childList.Count > 0);
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

      return WithField(field);
    }

    public FakeItem WithField(Field field)
    {
      string name = field.Name;
      string value = field.Value;
      ID fieldId = field.ID; 
      if (!string.IsNullOrEmpty(name))
      {
        this.Item.Fields[name].Returns(field);
      }

      if (this.Item.Fields[fieldId] == null)
      {
        var count = this.Item.Fields.Count;
        count++;
        this.Item.Fields.Count.Returns(count);
      }

      this.Item.Fields[fieldId].Returns(field);

      this.Item[fieldId].Returns(value);
      if (!string.IsNullOrEmpty(name))
      {
        this.Item[name].Returns(value);
      }

      var sectionItem = Substitute.For<TemplateSectionItem>(this.Item, this.Item.Template);
      var templateField = Substitute.For<TemplateFieldItem>(this.Item, sectionItem);

      if (!string.IsNullOrEmpty(name))
      {
        this.Item.Template.GetField(name).Returns(templateField);
      }

      this.Item.Template.GetField(fieldId).Returns(templateField);

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

    public FakeItem WithItemAccess(ItemAccess access)
    {
      this.Item.Access.Returns(access);
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
      Item.Database.GetItem(Item.ID.ToString(), Item.Language).Returns(Item);
      Item.Database.GetItem(Item.ID, Item.Language, Item.Version ?? Version.First).Returns(Item);
      Item.Database.GetItem(Item.ID.ToString(), Item.Language, Item.Version ?? Version.First).Returns(Item);
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

    public FakeItem WithAppearance()
    {
      var appearance = Substitute.For<ItemAppearance>(this.Item);
      return this.WithAppearance(appearance);
    }

    public FakeItem WithStatistics(ItemStatistics statistics)
    {
      Item.Statistics.Returns(statistics);

      return this;
    }

    public FakeItem WithStatistics()
    {
      var statistics = Substitute.For<ItemStatistics>(this.Item);
      return this.WithStatistics(statistics);
    }

    public FakeItem WithItemLinks(ItemLinks links)
    {
      Item.Links.Returns(links);

      return this;
    }

    public FakeItem WithItemLinks()
    {
      var links = Substitute.For<ItemLinks>(this.Item);
      return this.WithItemLinks(links);
    }

    public FakeItem WithItemLocking(ItemLocking itemLocking)
    {
      Item.Locking.Returns(itemLocking);

      return this;
    }

    public FakeItem WithItemLocking()
    {
      var locking = Substitute.For<ItemLocking>(this.Item);
      return this.WithItemLocking(locking);
    }

    public FakeItem WithItemVersions(ItemVersions versions)
    {
      Item.Versions.Returns(versions);

      return this;
    }

    public FakeItem WithItemVersions()
    {
      var versions = Substitute.For<ItemVersions>(this.Item);
      return this.WithItemVersions(versions);
    }

    public FakeItem WithItemAxes(ItemAxes axes)
    {
      Item.Axes.Returns(axes);

      return this;
    }

    public FakeItem WithItemAxes()
    {
      var axes = Substitute.For<ItemAxes>(this.Item);
      return this.WithItemAxes(axes);
    }

    public FakeItem WithItemEditing(ItemEditing itemEditing)
    {
      Item.Editing.Returns(itemEditing);

      return this;
    }

    public FakeItem WithItemEditing()
    {
      var editing = Substitute.For<ItemEditing>(this.Item);
      return this.WithItemEditing(editing);
    }

    public FakeItem WithBranch(BranchItem branch)
    {
      this.Item.Branch.Returns(branch);
      return this;
    }

    public FakeItem WithBranch()
    {
      var branch = Substitute.For<BranchItem>(this.Item);
      return this.WithBranch(branch);
    }

    public FakeItem WithBranchId(ID branchId)
    {
      this.Item.BranchId.Returns(branchId);
      return this;
    }

    public FakeItem WithBranches(BranchItem[] branches)
    {
      this.Item.Branches.Returns(branches);
      return this;
    }

    public FakeItem WithCreated(DateTime created)
    {
      this.Item.Created.Returns(created);
      return this;
    }

    public FakeItem WithDisplayName(string displayName)
    {
      this.Item.DisplayName.Returns(displayName);
      return this;
    }

    public FakeItem WithHasClones(bool hasClones)
    {
      this.Item.HasClones.Returns(hasClones);
      return this;
    }

    public FakeItem WithGetClones(IEnumerable<Item> clones)
    {
      this.Item.GetClones().Returns(clones);
      this.WithHasClones(clones.Count() > 0);
      return this;
    }

    public FakeItem WithHelp(ItemHelp help)
    {
      this.Item.Help.Returns(help);
      return this;
    }

    public FakeItem WithHelp()
    {
      var help = Substitute.For<ItemHelp>(this.Item);
      return this.WithHelp(help);
    }

    public FakeItem WithIsClone(bool isClone)
    {
      this.Item.IsClone.Returns(isClone);
      return this;
    }

    public FakeItem WithIsFallback(bool isFallback)
    {
      this.Item.IsFallback.Returns(isFallback);
      return this;
    }

    public FakeItem WithLanguages(Language[] languages)
    {
      this.Item.Languages.Returns(languages);
      return this;
    }

    public FakeItem WithLanguages(string[] languages)
    {
      var langs = new List<Language>();
      foreach (var lang in languages)
      {
        langs.Add(Language.Parse(lang));
      }

      return this.WithLanguages(langs.ToArray());
    }

    public FakeItem WithModified(bool modified)
    {
      this.Item.Modified.Returns(modified);
      return this;
    }

    public FakeItem WithOriginatorId(ID id)
    {
      this.Item.OriginatorId.Returns(id);
      return this;
    }

    public FakeItem WithOriginalLanguage(Language language)
    {
      this.Item.OriginalLanguage.Returns(language);
      return this;
    }

    public FakeItem WithOriginalLanguage(string language)
    {
      return this.WithOriginalLanguage(Language.Parse(language));
    }

    public FakeItem WithPublishing(ItemPublishing publishing)
    {
      this.Item.Publishing.Returns(publishing);
      return this;
    }

    public FakeItem WithPublishing()
    {
      return this.WithPublishing(Substitute.For<ItemPublishing>(this.Item));
    }

    public FakeItem WithRuntimeSettings(ItemRuntimeSettings runtime)
    {
      this.Item.RuntimeSettings.Returns(runtime);
      return this;
    }

    public FakeItem WithRuntimeSettings()
    {
      return this.WithRuntimeSettings(Substitute.For<ItemRuntimeSettings>(this.Item));
    }

    public FakeItem WithSecurity(ItemSecurity security)
    {
      this.Item.Security.Returns(security);
      return this;
    }

    public FakeItem WithSecurity()
    {
      return this.WithSecurity(Substitute.For<ItemSecurity>(this.Item));
    }

    public FakeItem WithSource(Item source)
    {
      this.Item.Source.Returns(source);
      return this;
    }

    public FakeItem WithSourceUri(ItemUri uri)
    {
      this.Item.SourceUri.Returns(uri);
      return this;
    }

    public FakeItem WithSourceUri()
    {
      return this.WithSourceUri(FakeUtil.FakeItemUri());
    }

    public FakeItem WithState(ItemState state)
    {
      this.Item.State.Returns(state);
      return this;
    }

    public FakeItem WithState()
    {
      return this.WithState(Substitute.For<ItemState>(this.Item));
    }

    public FakeItem WithTemplateName(string name)
    {
      this.Item.TemplateName.Returns(name);
      return this;
    }

    public FakeItem WithVisualization(ItemVisualization visualizations)
    {
      this.Item.Visualization.Returns(visualizations);
      return this;
    }

    public FakeItem WithVisualization()
    {
      return this.WithVisualization(Substitute.For<ItemVisualization>(this.Item));
    }

    public FakeItem WithIsItemClone(bool isClone)
    {
      this.Item.IsItemClone.Returns(isClone);
      return this;
    }

    public FakeItem WithSharedFieldsSource(Item source)
    {
      this.Item.SharedFieldsSource.Returns(source);
      return this;
    }
  }
}
