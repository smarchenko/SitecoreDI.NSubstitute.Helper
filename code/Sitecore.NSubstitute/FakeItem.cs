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
    using Sitecore.Globalization;

    using Version = Sitecore.Data.Version;

    public class FakeItem
    {
        private readonly ItemList childList = new ItemList();

        /// <summary>
        /// Initializes a new instance of <see cref="FakeItem"/> optionally providing item <paramref name="id"/> and <paramref name="database"/>.
        /// <para>Sets <see cref="Data.Items.Item.Language"/> (to <see cref="Language.Invariant"/>) and <see cref="Data.Items.Item.Version"/> (to <see cref="Version.First"/>).</para>
        /// <para>Fakes <see cref="Data.Items.Item.Paths"/>, <see cref="Data.Items.Item.Fields"/>, and <see cref="Data.Items.Item.Children"/> out-of-the-box.</para>
        /// <para>Configures <paramref name="database"/> to return this instance for GetItem calls.</para>
        /// </summary>
        /// <param name="id">The optional item id to use; otherwise random <see cref="Sitecore.Data.ID"/> is picked.</param>
        /// <param name="database">The optional database item belongs to; otherwise random <see cref="Database"/> is set as item owner.</param>
        public FakeItem(ID id = null, Database database = null)
        {
            Item = FakeUtil.FakeItem(id ?? ID.NewID, "fakeItem", database ?? FakeUtil.FakeDatabase());
            FakeUtil.FakeItemFields(Item);
            FakeUtil.FakeItemPath(Item);

            var templateItem = Substitute.For<TemplateItem>(Item);
            Item.Template.Returns(templateItem);
            Item.Language.Returns(Globalization.Language.Invariant);
            Item.Version.Returns(Version.First);

            Item.Children.Returns(Substitute.For<ChildList>(Item, childList));
            Item.GetChildren(Arg.Any<ChildListOptions>()).Returns(Substitute.For<ChildList>(Item, childList));
            Item.GetChildren().Returns(Substitute.For<ChildList>(Item, childList));
            Item.HasChildren.Returns(o => childList.Count > 0);
            Item.Database.GetItem(Item.ID).Returns(Item);
            Item.Database.GetItem(Item.ID.ToString()).Returns(Item);
            Item.Database.GetItem(Item.ID, Item.Language, Item.Version).Returns(Item);
            Item.Database.GetItem(Item.ID, Item.Language).Returns(Item);
        }

        /// <summary>
        /// The <see cref="Data.Items.Item.ID"/> of the item being built now.
        /// </summary>
        public ID ID => Item.ID;

        private Item Item { get; set; }

        public static implicit operator Item(FakeItem fakeItem) => fakeItem.Item;

        /// <summary>
        /// Converts current instance to Sitecore item.
        /// </summary>
        /// <returns></returns>
        public Item ToSitecoreItem() => Item;

        /// <summary>
        /// Adds a field with given <paramref name="id"/>, <paramref name="name"/> and <paramref name="value"/> to current item.
        /// </summary>
        /// <param name="id">The id of the field to be added.</param>
        /// <param name="name">The name of the field to be added.</param>
        /// <param name="value">The value of the field to be added.</param>
        public void Add(ID id, string name, string value) => WithField(id, name, value);

        /// <summary>
        /// Adds a field with given  <paramref name="name"/> and <paramref name="value"/> to current item.
        /// </summary>        
        /// <param name="name">The name of the field to be added.</param>
        /// <param name="value">The value of the field to be added.</param>
        public void Add(string name, string value) => WithField(name, value);

        /// <summary>
        /// Adds a field with given <paramref name="id"/>, <paramref name="value"/> to current item.
        /// </summary>
        /// <param name="id">The id of the field to be added.</param>        
        /// <param name="value">The value of the field to be added.</param>
        public void Add(ID id, string value) => WithField(id, value);

        public FakeItem WithTemplate(ID templateId)
        {
            Item.Template.ID.Returns(templateId);

            Item.TemplateID.Returns(templateId);

            var runtimeSettings = Substitute.For<ItemRuntimeSettings>(Item);
            runtimeSettings.TemplateDatabase.Returns(Item.Database);
            Item.RuntimeSettings.Returns(runtimeSettings);


            var engines = Substitute.For<DatabaseEngines>(Item.Database);
            var templateEngine = Substitute.For<TemplateEngine>(Item.Database);
            var template = new Template.Builder(templateId.ToString(), templateId, new TemplateCollection());

            templateEngine.GetTemplate(templateId).Returns(template.Template);

            engines.TemplateEngine.Returns(templateEngine);
            Item.Database.Engines.Returns(engines);
            Item.Database.GetTemplate(templateId).Returns(d => Item.Template);

            return this;
        }

        public FakeItem WithName(string name)
        {
            Item.Name.Returns(name);
            return this;
        }

        /// <summary>
        /// Adds item as a child, sets <paramref name="child"/> parent to current item.
        /// <para><see cref="Data.Items.Item.Children"/>, and <see cref="Data.Items.Item.GetChildren()"/> will have added <paramref name="child"/>.</para>
        /// </summary>
        /// <param name="child">The child item of current one.</param>
        /// <returns>Self to power builder-alike configuration.</returns>
        public FakeItem WithChild(FakeItem child)
        {
            childList.Add(child);

            child.Item.Parent.Returns(this);
            child.Item.ParentID.Returns(ID);

            return this;
        }

        /// <summary>
        /// Sets current item as a child of <paramref name="parent"/>.
        /// <para>Current item becomes a member of parent children.</para>
        /// </summary>
        /// <param name="parent">The item to be a parent for current one.</param>
        /// <returns></returns>
        public FakeItem WithParent(FakeItem parent)
        {
            parent.WithChild(this);

            return this;
        }

        /// <summary>
        /// Adds a field with <paramref name="name"/> and <paramref name="value"/> to current item (<see cref="Field.ID"/> is picked randomly).
        /// <para>Configures <see cref="Data.Items.Item"/> and <see cref="Data.Items.Item.Fields"/> indexers to find the field by name and <see cref="Field.ID"/>.</para>
        /// <para>Configures <see cref="Data.Items.Item.Template"/> to return <see cref="TemplateField"/> for by field name, id.</para>
        /// </summary>        
        /// <param name="name">The name of the field to be added.</param>
        /// <param name="value">The value of the field to be added.</param>
        /// <returns>Self to power builder-alike configuration.</returns>
        public FakeItem WithField(string name, string value) => WithField(ID.NewID, name, value);

        /// <summary>
        /// Adds a field with given <paramref name="id"/> and <paramref name="value"/> to current item.
        /// <para>Configures <see cref="Data.Items.Item"/> and <see cref="Data.Items.Item.Fields"/> indexers to find the field by name, <see cref="Field.ID"/>.</para>
        /// <para>Configures <see cref="Data.Items.Item.Template"/> to return <see cref="TemplateField"/> for by field name, id.</para>
        /// </summary>
        /// <param name="id">The id of the field to be added.</param>        
        /// <param name="value">The value of the field to be added.</param>
        /// <returns>Self to power builder-alike configuration.</returns>
        public FakeItem WithField(ID id, string value) => WithField(id, string.Empty, value);

        /// <summary>
        /// Adds a field with given <paramref name="id"/>, <paramref name="name"/> and <paramref name="value"/> to the item.
        /// <para>Configures <see cref="Data.Items.Item"/> and <see cref="Data.Items.Item.Fields"/> indexers to find the field by name, <see cref="Field.ID"/>.</para>
        /// <para>Configures <see cref="Data.Items.Item.Template"/> to return <see cref="TemplateField"/> for by field name, id.</para>
        /// </summary>
        /// <param name="id">The id of the field to be added.</param>
        /// <param name="name">The name of the field to be added.</param>
        /// <param name="value">The value of the field to be added.</param>
        /// <returns>Self to power builder-alike configuration.</returns>
        public FakeItem WithField(ID id, string name, string value)
        {
            var field = Substitute.For<Field>(id, Item);
            field.Name.Returns(name);
            field.Value.Returns(value);

            return WithField(field);
        }

        /// <summary>
        /// Adds a field to the item configuring <see cref="Data.Items.Item"/> and <see cref="Data.Items.Item.Fields"/> indexers to find the field by name, <see cref="Field.ID"/>.
        /// <para>Configures <see cref="Data.Items.Item.Template"/> to return <see cref="TemplateField"/> for by field name, id.</para>
        /// </summary>
        /// <returns>Self to power builder-alike configuration.</returns>
        public FakeItem WithField(Field field)
        {
            string name = field.Name;
            string value = field.Value;
            ID fieldId = field.ID;
            if (!string.IsNullOrEmpty(name))
            {
                Item.Fields[name].Returns(field);
            }

            if (Item.Fields[fieldId] == null)
            {
                var count = Item.Fields.Count;
                count++;
                Item.Fields.Count.Returns(count);
            }

            Item.Fields[fieldId].Returns(field);

            Item[fieldId].Returns(value);
            if (!string.IsNullOrEmpty(name))
            {
                Item[name].Returns(value);
            }

            var sectionItem = Substitute.For<TemplateSectionItem>(Item, Item.Template);
            var templateField = Substitute.For<TemplateFieldItem>(Item, sectionItem);

            if (!string.IsNullOrEmpty(name))
            {
                Item.Template.GetField(name).Returns(templateField);
            }

            Item.Template.GetField(fieldId).Returns(templateField);

            return this;
        }

        public void Add(FakeItem child) => WithChild(child);

        public FakeItem WithPath(string itemPath)
        {
            Item.Paths.FullPath.Returns(itemPath);
            Item.Database.GetItem(itemPath).Returns(Item);
            return this;
        }

        public FakeItem WithItemAccess()
        {
            FakeUtil.FakeItemAccess(Item);
            return this;
        }

        public FakeItem WithItemAccess(ItemAccess access)
        {
            Item.Access.Returns(access);
            return this;
        }

        public FakeItem WithUri()
        {
            var uri = Substitute.For<ItemUri>(Item.ID, Item.Paths == null ? string.Empty : Item.Paths.FullPath, Item.Language ?? Sitecore.Globalization.Language.Invariant, Item.Version ?? Version.Latest, Item.Database.Name);
            return WithUri(uri);
        }

        public FakeItem WithUri(ItemUri uri)
        {
            Item.Uri.Returns(uri);
            Item.Database.GetItem(Item.Uri.ToDataUri()).Returns(Item);
            return this;
        }

        public FakeItem WithLanguage(string languageName) => WithLanguage(Globalization.Language.Parse(languageName));

        public FakeItem WithLanguage(Sitecore.Globalization.Language language)
        {
            Item.Language.Returns(language);

            Item.Database.GetItem(Item.ID, Item.Language).Returns(Item);
            Item.Database.GetItem(Item.ID.ToString(), Item.Language).Returns(Item);
            Item.Database.GetItem(Item.ID, Item.Language, Item.Version ?? Version.First).Returns(Item);
            Item.Database.GetItem(Item.ID.ToString(), Item.Language, Item.Version ?? Version.First).Returns(Item);
            return this;
        }

        public FakeItem WithVersion(int number) => WithVersion(Version.Parse(number));

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

        public FakeItem WithAppearance() => WithAppearance(Substitute.For<ItemAppearance>(Item));

        public FakeItem WithStatistics(ItemStatistics statistics)
        {
            Item.Statistics.Returns(statistics);
            return this;
        }

        public FakeItem WithStatistics() => WithStatistics(Substitute.For<ItemStatistics>(Item));

        public FakeItem WithItemLinks(ItemLinks links)
        {
            Item.Links.Returns(links);
            return this;
        }

        public FakeItem WithItemLinks()
        {
            var links = Substitute.For<ItemLinks>(Item);
            return WithItemLinks(links);
        }

        public FakeItem WithItemLocking(ItemLocking itemLocking)
        {
            Item.Locking.Returns(itemLocking);
            return this;
        }

        public FakeItem WithItemLocking()
        {
            var locking = Substitute.For<ItemLocking>(Item);
            return WithItemLocking(locking);
        }

        public FakeItem WithItemVersions(ItemVersions versions)
        {
            Item.Versions.Returns(versions);
            return this;
        }

        public FakeItem WithItemVersions() => WithItemVersions(Substitute.For<ItemVersions>(Item));

        public FakeItem WithItemAxes(ItemAxes axes)
        {
            Item.Axes.Returns(axes);
            return this;
        }

        public FakeItem WithItemAxes() => WithItemAxes(Substitute.For<ItemAxes>(Item));

        public FakeItem WithItemEditing(ItemEditing itemEditing)
        {
            Item.Editing.Returns(itemEditing);

            return this;
        }

        public FakeItem WithItemEditing()
        {
            var editing = Substitute.For<ItemEditing>(Item);
            return WithItemEditing(editing);
        }

        public FakeItem WithBranch(BranchItem branch)
        {
            Item.Branch.Returns(branch);
            return this;
        }

        public FakeItem WithBranch() => WithBranch(Substitute.For<BranchItem>(Item));

        public FakeItem WithBranchId(ID branchId)
        {
            Item.BranchId.Returns(branchId);
            return this;
        }

        public FakeItem WithBranches(BranchItem[] branches)
        {
            Item.Branches.Returns(branches);
            return this;
        }

        public FakeItem WithCreated(DateTime created)
        {
            Item.Created.Returns(created);
            return this;
        }

        public FakeItem WithDisplayName(string displayName)
        {
            Item.DisplayName.Returns(displayName);
            return this;
        }

        public FakeItem WithHasClones(bool hasClones)
        {
            Item.HasClones.Returns(hasClones);
            return this;
        }

        public FakeItem WithGetClones(IEnumerable<Item> clones)
        {
            Item.GetClones().Returns(clones);
            WithHasClones(clones.Count() > 0);
            return this;
        }

        public FakeItem WithHelp(ItemHelp help)
        {
            Item.Help.Returns(help);
            return this;
        }

        public FakeItem WithHelp()
        {
            var help = Substitute.For<ItemHelp>(Item);
            return WithHelp(help);
        }

        public FakeItem WithIsClone(bool isClone)
        {
            Item.IsClone.Returns(isClone);
            return this;
        }

        public FakeItem WithIsFallback(bool isFallback)
        {
            Item.IsFallback.Returns(isFallback);
            return this;
        }

        public FakeItem WithLanguages(Language[] languages)
        {
            Item.Languages.Returns(languages);
            return this;
        }

        public FakeItem WithLanguages(string[] languages)
        {
            var parsedLanguages = new List<Language>();
            foreach (var lang in languages)
            {
                parsedLanguages.Add(Language.Parse(lang));
            }

            return WithLanguages(parsedLanguages.ToArray());
        }

        public FakeItem WithModified(bool modified)
        {
            Item.Modified.Returns(modified);
            return this;
        }

        public FakeItem WithOriginatorId(ID id)
        {
            Item.OriginatorId.Returns(id);
            return this;
        }

        public FakeItem WithOriginalLanguage(Language language)
        {
            Item.OriginalLanguage.Returns(language);
            return this;
        }

        public FakeItem WithOriginalLanguage(string language) => WithOriginalLanguage(Language.Parse(language));

        public FakeItem WithPublishing(ItemPublishing publishing)
        {
            Item.Publishing.Returns(publishing);
            return this;
        }

        public FakeItem WithPublishing() => WithPublishing(Substitute.For<ItemPublishing>(Item));

        public FakeItem WithRuntimeSettings(ItemRuntimeSettings runtime)
        {
            Item.RuntimeSettings.Returns(runtime);
            return this;
        }

        public FakeItem WithRuntimeSettings() => WithRuntimeSettings(Substitute.For<ItemRuntimeSettings>(Item));

        public FakeItem WithSecurity(ItemSecurity security)
        {
            Item.Security.Returns(security);
            return this;
        }

        public FakeItem WithSecurity() => WithSecurity(Substitute.For<ItemSecurity>(Item));

        public FakeItem WithSource(Item source)
        {
            Item.Source.Returns(source);
            return this;
        }

        public FakeItem WithSourceUri(ItemUri uri)
        {
            Item.SourceUri.Returns(uri);
            return this;
        }

        public FakeItem WithSourceUri() => WithSourceUri(FakeUtil.FakeItemUri());

        public FakeItem WithState(ItemState state)
        {
            Item.State.Returns(state);
            return this;
        }

        public FakeItem WithState() => WithState(Substitute.For<ItemState>(Item));

        public FakeItem WithTemplateName(string name)
        {
            Item.TemplateName.Returns(name);
            return this;
        }

        public FakeItem WithVisualization(ItemVisualization visualizations)
        {
            Item.Visualization.Returns(visualizations);
            return this;
        }

        public FakeItem WithVisualization() => WithVisualization(Substitute.For<ItemVisualization>(Item));

        public FakeItem WithIsItemClone(bool isClone)
        {
            Item.IsItemClone.Returns(isClone);
            return this;
        }

        public FakeItem WithSharedFieldsSource(Item source)
        {
            Item.SharedFieldsSource.Returns(source);
            return this;
        }
    }
}
