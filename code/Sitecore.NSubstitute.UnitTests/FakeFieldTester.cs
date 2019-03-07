using FluentAssertions;
using NSubstitute;
using Sitecore.Data;
using Sitecore.NSubstituteUtils;
using Xunit;

namespace Sitecore.NSubstitute.UnitTests
{
    public class FakeFieldTester
    {
        [Fact]
        public void DefaultConstructor_ShouldInitialize_UsingDefaultData()
        {
            var field = new FakeField();

            field.ToSitecoreField().Should().NotBeNull();
            var scField = field.ToSitecoreField();
            scField.ID.Should().NotBeNull();
            scField.ID.Should().NotBe(ID.Null);
            scField.Item.Should().NotBeNull();
            scField.Database.Should().NotBeNull();
        }

        [Fact]
        public void DatabaseProperty_IsTaken_FromItem()
        {
            var database = Substitute.For<Database>();
            database.Name.Returns("fake db name");

            var field = new FakeField(ID.NewID, new FakeItem(ID.NewID, database)).ToSitecoreField();
            field.Database.Name.Should().Be("fake db name");
        }

        [Fact]
        public void ItemProperty_ReturnedCorrectly()
        {
            var item = new FakeItem().ToSitecoreItem();
            var field = new FakeField(ID.NewID, item).ToSitecoreField();

            field.Item.Should().Be(item);
        }

        [Fact]
        public void FieldValue_CanBeMocked()
        {
            var field = new FakeField()
              .WithValue("test value")
              .ToSitecoreField();

            field.Value.Should().Be("test value");
        }

        [Fact]
        public void Constructor_AllowsToSetValue()
        {
            var field = new FakeField(ID.NewID, "test value", new FakeItem()).ToSitecoreField();

            field.Value.Should().Be("test value");
        }

        [Fact]
        public void FakeItemFields_ShouldBeModified()
        {
            var item = new FakeItem();
            item.ToSitecoreItem().Fields.Count.Should().Be(0);

            var field = new FakeField(ID.NewID, item).ToSitecoreField();

            item.ToSitecoreItem().Fields.Count.Should().Be(1);
            item.ToSitecoreItem().Fields[field.ID].Should().Be(field);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ValidationText()
        {
            var field = new FakeField()
              .WithValidationText("some text")
              .ToSitecoreField();

            field.ValidationText.Should().Be("some text");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Validation()
        {
            var field = new FakeField()
              .WithValidation("validation")
              .ToSitecoreField();

            field.Validation.Should().Be("validation");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Unversioned()
        {
            var field = new FakeField()
              .WithUnversioned(true)
              .ToSitecoreField();

            field.Unversioned.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_TypeKey()
        {
            var field = new FakeField()
              .WithTypeKey("type key")
              .ToSitecoreField();

            field.TypeKey.Should().Be("type key");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Type()
        {
            var field = new FakeField()
              .WithType("type")
              .ToSitecoreField();

            field.Type.Should().Be("type");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Translatable()
        {
            var field = new FakeField()
              .WithTranslatable(true)
              .ToSitecoreField();

            field.Translatable.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ToolTip()
        {
            var field = new FakeField()
              .WithToolTip("tooltip")
              .ToSitecoreField();

            field.ToolTip.Should().Be("tooltip");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Title()
        {
            var field = new FakeField()
              .WithTitle("title")
              .ToSitecoreField();

            field.Title.Should().Be("title");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Style()
        {
            var field = new FakeField()
              .WithStyle("style")
              .ToSitecoreField();

            field.Style.Returns("style");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Source()
        {
            var field = new FakeField()
              .WithSource("source")
              .ToSitecoreField();

            field.Source.Should().Be("source");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Sortorder()
        {
            var field = new FakeField()
              .WithSortorder(5)
              .ToSitecoreField();

            field.Sortorder.Should().Be(5);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ShouldBeTranslated()
        {
            var field = new FakeField()
              .WithShouldBeTranslated(true)
              .ToSitecoreField();

            field.ShouldBeTranslated.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Shared()
        {
            var field = new FakeField()
              .WithShared(true)
              .ToSitecoreField();

            field.Shared.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_SectionSortorder()
        {
            var field = new FakeField()
              .WithSectionSortorder(5)
              .ToSitecoreField();

            field.SectionSortorder.Should().Be(5);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_SectionNameByUILocale()
        {
            var field = new FakeField()
              .WithSectionNameByUILocale("name")
              .ToSitecoreField();

            field.SectionNameByUILocale.Should().Be("name");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Section()
        {
            var field = new FakeField()
              .WithSection("section")
              .ToSitecoreField();

            field.Section.Should().Be("section");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ResetBlank()
        {
            var field = new FakeField()
              .WithResetBlank(true)
              .ToSitecoreField();

            field.ResetBlank.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Name()
        {
            var field = new FakeField()
              .WithName("name")
              .ToSitecoreField();

            field.Name.Returns("name");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_SharedLanguageFallbackEnabled()
        {
            var field = new FakeField()
              .WithSharedLanguageFallbackEnabled(true)
              .ToSitecoreField();

            field.SharedLanguageFallbackEnabled.Should().BeTrue();
        }

        [Fact]
        public void FakeField_Shouldsubstitute_Language()
        {
            var field = new FakeField()
              .WithLanguage("ru-RU")
              .ToSitecoreField();

            field.Language.Name.Should().Be("ru-RU");
        }

        [Fact]
        public void FakeField_ShouldTakeLanguage_FromItem()
        {
            var item = new FakeItem()
              .WithLanguage("ru-RU")
              .ToSitecoreItem();

            var field = new FakeField(owner: item)
              .ToSitecoreField();

            field.Language.Name.Should().Be("ru-RU");
        }

        [Fact]
        public void FakeItem_ShouldSubstitiute_Key()
        {
            var field = new FakeField()
              .WithKey("key")
              .ToSitecoreField();

            field.Key.Returns("key");
        }

        [Fact]
        public void FakeItem_ShouldSubstitute_IsModified()
        {
            var field = new FakeField()
              .WithIsModified(true)
              .ToSitecoreField();

            field.IsModified.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_IsBlobField()
        {
            var field = new FakeField()
              .WithIsBlobField(true)
              .ToSitecoreField();

            field.IsBlobField.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_InheritedValue()
        {
            var field = new FakeField()
              .WithInheritedValue("value")
              .ToSitecoreField();

            field.InheritedValue.Should().Be("value");
        }

        [Fact]
        public void FakeField_ShouldSubstitiute_HelpLink()
        {
            var field = new FakeField()
              .WithHelpLink("link")
              .ToSitecoreField();

            field.HelpLink.Should().Be("link");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_HasValue()
        {
            var field = new FakeField()
              .WithHasValue(true)
              .ToSitecoreField();

            field.HasValue.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_HasBlobStream()
        {
            var field = new FakeField()
              .WithHasBlobStream(true)
              .ToSitecoreField();

            field.HasBlobStream.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_SectionDisplayName()
        {
            var field = new FakeField()
              .WithSectionDisplayName("name")
              .ToSitecoreField();

            field.SectionDisplayName.Should().Be("name");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_DisplayName()
        {
            var field = new FakeField()
              .WithDisplayName("name")
              .ToSitecoreField();

            field.DisplayName.Should().Be("name");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Description()
        {
            var field = new FakeField()
              .WithDescription("description")
              .ToSitecoreField();

            field.Description.Should().Be("description");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Definition()
        {
            var field = new FakeField()
              .WithDefinition()
              .ToSitecoreField();

            field.Definition.Should().NotBeNull();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Definition1()
        {
            var template = new FakeTemplate();
            var section = new FakeTemplateSection(template, "fakeSection", ID.NewID);
            var templateField = new FakeTemplateField(section, "fakeName", ID.NewID).ToSitecoreTemplateField();

            var field = new FakeField()
              .WithDefinition(templateField)
              .ToSitecoreField();

            field.Definition.Should().Be(templateField);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_InnerItem()
        {
            var item = new FakeItem().ToSitecoreItem();
            var field = new FakeField()
              .WithInnerItem(item)
              .ToSitecoreField();

            field.InnerItem.Should().Be(item);
            field.Database.GetItem(field.ID, null).Should().Be(item);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_FallbackValueSource()
        {
            var field = new FakeField()
              .WithFallbackValueSource("source")
              .ToSitecoreField();

            field.FallbackValueSource.Should().Be("source");
        }

        [Fact]
        public void FakeField_ShouldSubstitute_InheritsValueFromOtherItem()
        {
            var field = new FakeField()
              .WithInheritsValueFromOtherItem(true)
              .ToSitecoreField();

            field.InheritsValueFromOtherItem.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ContainsFallbackValue()
        {
            var field = new FakeField()
              .WithContainsFallbackValue(true)
              .ToSitecoreField();

            field.ContainsFallbackValue.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ContainsStandardValue()
        {
            var field = new FakeField()
              .WithContainsStandardValue(true)
              .ToSitecoreField();

            field.ContainsStandardValue.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_CanWrite()
        {
            var field = new FakeField()
              .WithCanWrite(true)
              .ToSitecoreField();

            field.CanWrite.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_CanRead()
        {
            var field = new FakeField()
              .WithCanRead(true)
              .ToSitecoreField();

            field.CanRead.Should().BeTrue();
        }
    }
}
