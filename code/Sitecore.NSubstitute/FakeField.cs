using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.NSubstituteUtils
{
  using NSubstitute;
  using NSubstitute.ReturnsExtensions;

  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Data.Templates;
  using Sitecore.Globalization;

  public class FakeField
  {
    private FakeField fakeField;

    public FakeField(ID fieldId, FakeItem item)
      :this(fieldId, item ?? new FakeItem().ToSitecoreItem())
    {
      item.WithField(this.Field);
    }

    public FakeField(ID fieldId = null, Item owner = null)
    {
      var item = owner ?? new FakeItem().ToSitecoreItem();
      this.Field = Substitute.For<Field>(fieldId ?? ID.NewID, item);
      this.Field.Database.Returns(this.Field.Item.Database);
      this.WithLanguage(item.Language);
    }

    public FakeField(ID fieldId, string value, Item owner)
      :this(fieldId, owner)
    {
      this.WithValue(value);
    }

    private Field Field { get; set; }

    public FakeField WithValue(string value)
    {
      this.Field.Value.Returns(value);
      return this;
    }

    public FakeField WithValidationText(string text)
    {
      this.Field.ValidationText.Returns(text);
      return this;
    }

    public FakeField WithValidation(string validation)
    {
      this.Field.Validation.Returns(validation);
      return this;
    }

    public FakeField WithUnversioned(bool unversioned)
    {
      this.Field.Unversioned.Returns(unversioned);
      return this;
    }

    public FakeField WithTypeKey(string typeKey)
    {
      this.Field.TypeKey.Returns(typeKey);
      return this;
    }

    public FakeField WithType(string type)
    {
      this.Field.Type.Returns(type);
      return this;
    }

    public FakeField WithTranslatable(bool translatable)
    {
      this.Field.Translatable.Returns(translatable);
      return this;
    }

    public FakeField WithToolTip(string toolTip)
    {
      this.Field.ToolTip.Returns(toolTip);
      return this;
    }

    public FakeField WithTitle(string title)
    {
      this.Field.Title.Returns(title);
      return this;
    }

    public FakeField WithStyle(string style)
    {
      this.Field.Style.Returns(style);
      return this;
    }

    public FakeField WithSource(string source)
    {
      this.Field.Source.Returns(source);
      return this;
    }

    public FakeField WithSortorder(int sortorder)
    {
      this.Field.Sortorder.Returns(sortorder);
      return this;
    }

    public FakeField WithShouldBeTranslated(bool shouldBeTranslated)
    {
      this.Field.ShouldBeTranslated.Returns(shouldBeTranslated);
      return this;
    }

    public FakeField WithShared(bool shared)
    {
      this.Field.Shared.Returns(shared);
      return this;
    }

    public FakeField WithSectionSortorder(int order)
    {
      this.Field.SectionSortorder.Returns(order);
      return this;
    }

    public FakeField WithSectionNameByUILocale(string name)
    {
      this.Field.SectionNameByUILocale.Returns(name);
      return this;
    }

    public FakeField WithSection(string section)
    {
      this.Field.Section.Returns(section);
      return this;
    }

    public FakeField WithResetBlank(bool reset)
    {
      this.Field.ResetBlank.Returns(reset);
      return this;
    }

    public FakeField WithName(string name)
    {
      this.Field.Name.Returns(name);
      return this;
    }

    public FakeField WithSharedLanguageFallbackEnabled(bool sharedFallbackEnabled)
    {
      this.Field.SharedLanguageFallbackEnabled.Returns(sharedFallbackEnabled);
      return this;
    }

    public FakeField WithLanguage(Language language)
    {
      this.Field.Language.Returns(language);
      return this;
    }

    public FakeField WithLanguage(string language)
    {
      return WithLanguage(Language.Parse(language));
    }

    public FakeField WithKey(string key)
    {
      this.Field.Key.Returns(key);
      return this;
    }

    public FakeField WithIsModified(bool modified)
    {
      this.Field.IsModified.Returns(modified);
      return this;
    }

    public FakeField WithIsBlobField(bool isBlob)
    {
      this.Field.IsBlobField.Returns(isBlob);
      return this;
    }

    public FakeField WithInheritedValue(string value)
    {
      this.Field.InheritedValue.Returns(value);
      return this;
    }

    public FakeField WithHelpLink(string helpLink)
    {
      this.Field.HelpLink.Returns(helpLink);
      return this;
    }

    public FakeField WithHasValue(bool hasValue)
    {
      this.Field.HasValue.Returns(hasValue);
      return this;
    }

    public FakeField WithHasBlobStream(bool hasStream)
    {
      this.Field.HasBlobStream.Returns(hasStream);
      return this;
    }

    public FakeField WithSectionDisplayName(string name)
    {
      this.Field.SectionDisplayName.Returns(name);
      return this;
    }

    public FakeField WithDisplayName(string name)
    {
      this.Field.DisplayName.Returns(name);
      return this;
    }

    public FakeField WithDescription(string description)
    {
      this.Field.Description.Returns(description);
      return this;
    }

    public FakeField WithDefinition(TemplateField templateField)
    {
      this.Field.Definition.Returns(templateField);
      return this;
    }

    public FakeField WithDefinition()
    {
      var field = new FakeTemplateField().ToSitecoreTemplateField();
      this.Field.Definition.Returns(field);
      return this;
    }

    public FakeField WithInnerItem(Item item)
    {
      this.Field.InnerItem.Returns(item);
      this.Field.Database.GetItem(this.Field.ID, Arg.Any<Language>()).Returns(item);
      return this;
    }

    public FakeField WithFallbackValueSource(string source)
    {
      this.Field.FallbackValueSource.Returns(source);
      return this;
    }

    public FakeField WithInheritsValueFromOtherItem(bool inherited)
    {
      this.Field.InheritsValueFromOtherItem.Returns(inherited);
      return this;
    }

    public FakeField WithContainsFallbackValue(bool containsFallback)
    {
      this.Field.ContainsFallbackValue.Returns(containsFallback);
      return this;
    }

    public FakeField WithContainsStandardValue(bool contains)
    {
      this.Field.ContainsStandardValue.Returns(contains);
      return this;
    }

    public FakeField WithCanWrite(bool canWrite)
    {
      this.Field.CanWrite.Returns(canWrite);
      return this;
    }

    public FakeField WithCanRead(bool canRead)
    {
      this.Field.CanRead.Returns(canRead);
      return this;
    }

    public static implicit operator Field(FakeField fakeField)
    {
      return fakeField.Field;
    }

    public Field ToSitecoreField()
    {
      return this.Field;
    }
  }
}
