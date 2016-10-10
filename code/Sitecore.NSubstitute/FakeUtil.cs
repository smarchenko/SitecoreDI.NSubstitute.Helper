using System.IO;
using NSubstitute;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Locking;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Security.AccessControl;

namespace Sitecore.NSubstituteUtils
{
  public static class FakeUtil
  {
    #region FakeDatabase

    /// <summary>
    /// Creates a substitute for the Sitecore.Data.Database with specified name.
    /// </summary>
    /// <param name="name">The substitute database name.</param>
    /// <returns>Substitute for the Sitecore.Data.Database.</returns>
    public static Database FakeDatabase(string name)
    {
      var database = Substitute.For<Database>();
      database.Name.Returns(name);
      return database;
    }

    /// <summary>
    /// Creates a substitute for the Sitecore.Data.Database with default name "fakeDB".
    /// </summary>
    /// <returns>Substitute for the Sitecore.Data.Database.</returns>
    public static Database FakeDatabase()
    {
      return FakeDatabase("fakeDB");
    }

    #endregion FakeDatabase

    #region FakeItem

    public static Item FakeItem(ID itemId, string name, Database fakeDatabase)
    {
      var item = Substitute.For<Item>(itemId, ItemData.Empty, fakeDatabase);
      item.Name.Returns(name);
      return item;
    }

    public static Item FakeItem(string name, Database fakeDatabase)
    {
      return FakeItem(ID.NewID, name, fakeDatabase);
    }

    public static Item FakeItem(Database fakeDatabase)
    {
      return FakeItem("fakeItem", fakeDatabase);
    }

    public static Item FakeItem(string name)
    {
      return FakeItem(name, FakeUtil.FakeDatabase());
    }

    public static Item FakeItem()
    {
      return FakeItem("fakeItem");
    }

    #endregion FakeItem

    #region FakeFields

    public static Field FakeField(ID id, Item fakeItem)
    {
      var field = Substitute.For<Field>(id, fakeItem);
      field.Database.Returns(fakeItem.Database);
      return field;
    }

    public static Field FakeField(Item fakeItem)
    {
      return FakeField(Substitute.For<ID>(), fakeItem);
    }

    public static Item FakeItemFields(Item fakeItem)
    {
      var fieldCollection = Substitute.For<FieldCollection>(fakeItem);
      fakeItem.Fields.Returns(fieldCollection);
      return fakeItem;
    }

    public static Item FakeFieldValue(string name, string value, Item fakeItem)
    {
      return FakeFieldValue(ID.NewID, name, value, fakeItem);
    }

    public static Item FakeFieldValue(ID id, string name, string value, Item fakeItem)
    {
      if (fakeItem.Fields == null)
      {
        throw new InvalidDataException("Item field collection has not been substitute. Please use FakeItemFields before faking specific field.");
      }

      var field = Substitute.For<Field>(id, fakeItem);
      field.Value.Returns(value);
      fakeItem.Fields[name].Returns(field);
      fakeItem.Fields[id].Returns(field);
      return fakeItem;
    }

    #endregion FakeFields

    public static Item FakeItemPath(Item fakeItem)
    {
      var itemPath = Substitute.For<ItemPath>(fakeItem);
      fakeItem.Paths.Returns(itemPath);
      return fakeItem;
    }

    public static Item FakeItemAccess(Item fakeItem)
    {
      var itemAccess = Substitute.For<ItemAccess>(fakeItem);
      fakeItem.Access.Returns(itemAccess);
      return fakeItem;
    }

    public static ItemUri FakeItemUri()
    {
      return FakeItemUri(ID.NewID, "", Language.Invariant, Version.Latest, "fakeDatabase");
    }

    public static ItemUri FakeItemUri(ID id, string path, Language language, Version version, string databaseName)
    {
      return Substitute.For<ItemUri>(id, path, language, version, databaseName);
    }

    public static Item FakeItemUri(Item fakeItem)
    {
      var uri = Substitute.For<ItemUri>(fakeItem.ID, fakeItem.ID.ToString(), fakeItem.Language ?? Language.Invariant, fakeItem.Version ?? Version.Latest, fakeItem.Database.Name);
      fakeItem.Uri.Returns(uri);
      return fakeItem;
    }

    public static DataUri FakeDataUri()
    {
      return FakeDataUri(ID.NewID, Language.Invariant, Version.Latest);
    }

    public static DataUri FakeDataUri(ID id, Language language, Version version)
    {
      return Substitute.For<DataUri>(id, language, version);
    }

    public static Item FakeItemAppearance(Item fakeItem)
    {
      var itemAppearance = Substitute.For<ItemAppearance>(fakeItem);
      fakeItem.Appearance.Returns(itemAppearance);
      return fakeItem;
    }

    public static Item FakeItemStatistics(Item fakeItem)
    {
      var itemStatistics = Substitute.For<ItemStatistics>(fakeItem);
      fakeItem.Statistics.Returns(itemStatistics);
      return fakeItem;
    }

    public static Item FakeItemTemplate(Item fakeItem)
    {
      var templateItem = Substitute.For<TemplateItem>(fakeItem);
      fakeItem.Template.Returns(templateItem);
      return fakeItem;
    }

    public static Item FakeItemLanguage(Item fakeItem)
    {
      var language = Substitute.For<Language>();
      fakeItem.Language.Returns(language);
      return fakeItem;
    }

    public static Item FakeItemLinks(Item fakeItem)
    {
      var links = Substitute.For<ItemLinks>(fakeItem);
      fakeItem.Links.Returns(links);
      return fakeItem;
    }

    public static Item FakeItemLocking(Item fakeItem)
    {
      var locking = Substitute.For<ItemLocking>(fakeItem);
      fakeItem.Locking.Returns(locking);
      return fakeItem;
    }

    public static Item FakeItemVersions(Item fakeItem)
    {
      var itemVersions = Substitute.For<ItemVersions>(fakeItem);
      fakeItem.Versions.Returns(itemVersions);
      return fakeItem;
    }

    public static Item FakeItemAxes(Item fakeItem)
    {
      var itemAxes = Substitute.For<ItemAxes>(fakeItem);
      fakeItem.Axes.Returns(itemAxes);
      return fakeItem;
    }

    public static Item FakeItemEditing(Item fakeItem)
    {
      var itemEditing = Substitute.For<ItemEditing>(fakeItem);
      fakeItem.Editing.Returns(itemEditing);
      return fakeItem;
    }
  }
}
