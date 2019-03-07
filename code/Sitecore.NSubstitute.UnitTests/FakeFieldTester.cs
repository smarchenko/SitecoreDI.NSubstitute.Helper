using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Templates;
using Sitecore.NSubstituteUtils;
using Xunit;

namespace Sitecore.NSubstitute.UnitTests
{
    public class FakeFieldTester
    {
        [Fact]
        public void DefaultConstructor_ShouldInitialize_UsingDefaultData()
        {
            Field field = new FakeField();

            field.ID.Should()
                .NotBeNull()
                .And
                .NotBe(ID.Null);
            
            field.Item.Should().NotBeNull();
            field.Database.Should().NotBeNull();
        }

        [Fact]
        public void DatabaseProperty_IsTaken_FromItem()
        {
            var database = Substitute.For<Database>();
            database.Name.Returns("fake db name");

            Field field = new FakeField(ID.NewID, new FakeItem(ID.NewID, database));
            field.Database.Name.Should().Be("fake db name");
        }

        [Fact]
        public void ItemProperty_WhenRequested_ReturnsFieldOwnerItem()
        {
            Item fieldOwnerItem = new FakeItem();

            Field field = new FakeField(ID.NewID, owner: fieldOwnerItem);

            field.Item.Should().Be(fieldOwnerItem);
        }

        [Theory, AutoData]        
        [InlineData("test value")]
        public void FieldValue_WhenMocked_ReturnsMockedValue(string fieldValue)
        {
            Field field = new FakeField().WithValue(fieldValue);

            field.Value.Should().Be(fieldValue);
        }

        [Theory, AutoData]        
        [InlineData("test field value")]
        public void Constructor_WhenReceivesFieldValue_SetsFieldValue(string fieldValue)
        {
            Field field = new FakeField(ID.NewID, fieldValue, new FakeItem());

            field.Value.Should().Be(fieldValue);
        }

        [Theory, AutoData]
        public void FakeItemFields_ShouldBeModified(ID fieldId)
        {
            var fakeItem = new FakeItem();
            var item = fakeItem.ToSitecoreItem();
            item.Fields.Should().BeEmpty();

            Field field = new FakeField(fieldId, fakeItem);

            item.Fields.Should().HaveCount(1);                            
            item.Fields[field.ID].Should().Be(field);
        }

        [Theory, AutoData]        
        [InlineData("validation text")]
        public void FakeField_ShouldSubstitute_ValidationText(string validationText)
        {
            Field field = new FakeField().WithValidationText(validationText);

            field.ValidationText.Should().Be(validationText);
        }

        [Theory, AutoData]        
        [InlineData("validation")]
        public void FakeField_ShouldSubstitute_Validation(string validation)
        {
            Field field = new FakeField().WithValidation(validation);

            field.Validation.Should().Be(validation);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Unversioned()
        {
            Field field = new FakeField().WithUnversioned(true);

            field.Unversioned.Should().BeTrue();
        }

        [Theory, AutoData]
        [InlineData("validation")]
        public void FakeField_ShouldSubstitute_TypeKey(string typeKey)
        {
            Field field = new FakeField().WithTypeKey(typeKey);

            field.TypeKey.Should().Be(typeKey);
        }

        [Theory, AutoData]        
        [InlineData("type")]
        public void FakeField_ShouldSubstitute_Type(string fieldType)
        {
            Field field = new FakeField().WithType(fieldType);
              
            field.Type.Should().Be(fieldType);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Translatable()
        {
            Field field = new FakeField().WithTranslatable(true);

            field.Translatable.Should().BeTrue();
        }

        [Theory, AutoData]
        [InlineData("tooltip")]
        public void FakeField_ShouldSubstitute_ToolTip(string toolTip)
        {
            Field field = new FakeField().WithToolTip(toolTip);

            field.ToolTip.Should().Be(toolTip);
        }

        [Theory, AutoData]
        [InlineData("title")]
        public void FakeField_ShouldSubstitute_Title(string title)
        {
            Field field = new FakeField().WithTitle(title);

            field.Title.Should().Be(title);
        }

        [Theory, AutoData]
        [InlineData("style")]
        public void FakeField_ShouldSubstitute_Style(string fieldStyle)
        {
            Field field = new FakeField().WithStyle(fieldStyle);

            field.Style.Returns(fieldStyle);
        }

        [Theory, AutoData]
        [InlineData("source")]
        public void FakeField_ShouldSubstitute_Source(string fieldSource)
        {
            Field field = new FakeField().WithSource(fieldSource);

            field.Source.Should().Be(fieldSource);
        }

        [Theory, AutoData]
        [InlineData(5)]
        public void FakeField_ShouldSubstitute_SortOrder(int sortOrder)
        {
            Field field = new FakeField().WithSortorder(sortOrder);

            field.Sortorder.Should().Be(sortOrder);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ShouldBeTranslated()
        {
            Field field = new FakeField().WithShouldBeTranslated(true);

            field.ShouldBeTranslated.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Shared()
        {
            Field field = new FakeField().WithShared(true);

            field.Shared.Should().BeTrue();
        }

        [Theory, AutoData]
        [InlineData(5)]
        public void FakeField_ShouldSubstitute_SectionSortOrder(int sectionSortOrder)
        {
            Field field = new FakeField().WithSectionSortorder(sectionSortOrder);

            field.SectionSortorder.Should().Be(sectionSortOrder);
        }

        [Theory, AutoData]
        [InlineData("name")]
        public void FakeField_ShouldSubstitute_SectionNameByUILocale(string sectionNameByUiLocale)
        {
            Field field = new FakeField().WithSectionNameByUILocale(sectionNameByUiLocale);

            field.SectionNameByUILocale.Should().Be(sectionNameByUiLocale);
        }

        [Theory, AutoData]
        [InlineData("section")]
        public void FakeField_ShouldSubstitute_Section(string sectionName)
        {
            Field field = new FakeField().WithSection(sectionName);

            field.Section.Should().Be(sectionName);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ResetBlank()
        {
            Field field = new FakeField().WithResetBlank(true);

            field.ResetBlank.Should().BeTrue();
        }

        [Theory, AutoData]
        [InlineData("name")]
        public void FakeField_ShouldSubstitute_Name(string fieldName)
        {
            Field field = new FakeField().WithName(fieldName);

            field.Name.Returns(fieldName);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_SharedLanguageFallbackEnabled()
        {
            Field field = new FakeField().WithSharedLanguageFallbackEnabled(true);

            field.SharedLanguageFallbackEnabled.Should().BeTrue();
        }
        
        [Theory, InlineData("ru-RU")]
        public void FakeField_ShouldSubstitute_Language(string languageName)
        {
            Field field = new FakeField().WithLanguage(languageName);

            field.Language.Name.Should().Be(languageName);
        }

        [Theory, InlineData("ru-RU")]
        public void FakeField_ShouldTakeLanguage_FromItem(string languageName)
        {
            var item = new FakeItem().WithLanguage(languageName).ToSitecoreItem();

            Field field = new FakeField(owner: item)
              ;

            field.Language.Name.Should().Be(languageName);
        }

        [Theory, AutoData]
        [InlineData("key")]
        public void FakeItem_ShouldSubstitute_Key(string key)
        {
            Field field = new FakeField().WithKey(key);

            field.Key.Returns(key);
        }

        [Fact]
        public void FakeItem_ShouldSubstitute_IsModified()
        {
            Field field = new FakeField().WithIsModified(true);

            field.IsModified.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_IsBlobField()
        {
            Field field = new FakeField().WithIsBlobField(true);

            field.IsBlobField.Should().BeTrue();
        }

        [Theory, AutoData]
        [InlineData("inherited value")]
        public void FakeField_ShouldSubstitute_InheritedValue(string inheritedValue)
        {
            Field field = new FakeField().WithInheritedValue(inheritedValue);

            field.InheritedValue.Should().Be(inheritedValue);
        }

        [Theory, AutoData]
        [InlineData("help link")]
        public void FakeField_ShouldSubstitute_HelpLink(string helpLink)
        {
            Field field = new FakeField().WithHelpLink(helpLink);

            field.HelpLink.Should().Be(helpLink);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_HasValue()
        {
            Field field = new FakeField().WithHasValue(true);

            field.HasValue.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_HasBlobStream()
        {
            Field field = new FakeField().WithHasBlobStream(true);

            field.HasBlobStream.Should().BeTrue();
        }

        [Theory, AutoData]
        [InlineData("section display name")]
        public void FakeField_ShouldSubstitute_SectionDisplayName(string sectionDisplayName)
        {
            Field field = new FakeField().WithSectionDisplayName(sectionDisplayName);

            field.SectionDisplayName.Should().Be(sectionDisplayName);
        }

        [Theory, AutoData]
        [InlineData("field display name")]
        public void FakeField_ShouldSubstitute_DisplayName(string fieldDisplayName)
        {
            Field field = new FakeField().WithDisplayName(fieldDisplayName);

            field.DisplayName.Should().Be(fieldDisplayName);
        }

        [Theory, AutoData]
        [InlineData("field description")]
        public void FakeField_ShouldSubstitute_Description(string description)
        {
            Field field = new FakeField().WithDescription(description);

            field.Description.Should().Be(description);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_Definition()
        {
            Field field = new FakeField().WithDefinition();

            field.Definition.Should().NotBeNull();
        }

        [Theory, InlineAutoData("fake section name", "fake template field name")]
        [AutoData]
        public void FakeField_ShouldSubstitute_Definition1(string sectionName, string fieldName, ID sectionId, ID fieldId)
        {
            var template = new FakeTemplate();
            var section = new FakeTemplateSection(template, sectionName, sectionId);
            TemplateField templateField = new FakeTemplateField(section, fieldName, fieldId);

            Field field = new FakeField().WithDefinition(templateField);

            field.Definition.Should().Be(templateField);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_InnerItem()
        {
            Item item = new FakeItem();
            Field field = new FakeField().WithInnerItem(item);

            field.InnerItem.Should().Be(item);
            field.Database.GetItem(field.ID, language: null).Should().Be(item);
        }

        [Theory, AutoData]
        [InlineData("fallback value source")]
        public void FakeField_ShouldSubstitute_FallbackValueSource(string source)
        {
            Field field = new FakeField().WithFallbackValueSource(source);

            field.FallbackValueSource.Should().Be(source);
        }

        [Fact]
        public void FakeField_ShouldSubstitute_InheritsValueFromOtherItem()
        {
            Field field = new FakeField().WithInheritsValueFromOtherItem(true);

            field.InheritsValueFromOtherItem.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ContainsFallbackValue()
        {
            Field field = new FakeField().WithContainsFallbackValue(true);

            field.ContainsFallbackValue.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_ContainsStandardValue()
        {
            Field field = new FakeField().WithContainsStandardValue(true);

            field.ContainsStandardValue.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_CanWrite()
        {
            Field field = new FakeField().WithCanWrite(true);

            field.CanWrite.Should().BeTrue();
        }

        [Fact]
        public void FakeField_ShouldSubstitute_CanRead()
        {
            Field field = new FakeField().WithCanRead(true);

            field.CanRead.Should().BeTrue();
        }
    }
}
