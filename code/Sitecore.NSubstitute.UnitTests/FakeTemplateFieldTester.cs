using AutoFixture.Xunit2;
using FluentAssertions;
using Sitecore.Data;
using Sitecore.Data.Templates;
using Sitecore.Globalization;
using Sitecore.NSubstituteUtils;
using Xunit;

namespace Sitecore.NSubstitute.UnitTests
{
    public class FakeTemplateFieldTester
    {
        [Fact]
        public void Constructor_WhenCalledWithoutValues_SetsNameAndId()
        {
            var fakeField = new FakeTemplateField();
            fakeField.Builder.Should().NotBeNull();
            TemplateField field = fakeField;

            field.Should().NotBeNull();
            field.Name.Should().Be("fakeField");
            field.ID.Should().NotBeNull();
        }

        [AutoData]
        [Theory, InlineAutoData("test name")]
        public void Constructor_WhenCalledWithNameAndId_SetsNameAndId(string name, ID id)
        {
            TemplateField field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()), name, id);

            field.Name.Should().Be(name);
            field.ID.Should().Be(id);
        }

        [AutoData]
        [Theory, InlineData("test icon")]
        public void WithIcon_WhenCalled_SetsIcon(string icon)
        {             
            TemplateField field = new FakeTemplateField().WithIcon(icon);

            field.Icon.Should().Be(icon);
        }

        [AutoData]
        [Theory, InlineData("test type")]
        public void WithType_WhenCalled_SetType(string fieldType)
        {
            TemplateField field = new FakeTemplateField().WithType(fieldType);

            field.Type.Should().Be(fieldType);
        }

        [AutoData]
        [Theory, InlineData("test style")]
        public void WithStyle_WhenCalled_SetStyle(string fieldStyle)
        {             
            TemplateField field = new FakeTemplateField().WithStyle(fieldStyle);

            field.Style.Should().Be(fieldStyle);
        }

        [Fact]
        public void FakeTemplateField_SetTitle()
        {
            string fieldTitle = "test title";
            Language language = Language.Parse("da-DK");
            TemplateField field = new FakeTemplateField()
              .WithTitle(fieldTitle, language)
              ;

            field.GetTitle(language).Should().Be(fieldTitle);
            field.GetTitle(Language.Parse("en")).Should().NotBe(fieldTitle);
            field.GetTitle(Language.Parse("en")).Should().Be("");
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithIsBlob_WhenCalled_SetIsBlob(bool isBlob)
        {             
            TemplateField field = new FakeTemplateField().WithIsBlob(isBlob);

            field.IsBlob.Should().Be(isBlob);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithIsShared_WhenCalled_SetIsShared(bool isShared)
        {             
            TemplateField field = new FakeTemplateField().WithIsShared(isShared);

            field.IsShared.Should().Be(isShared);
        }

        [AutoData]
        [Theory, InlineData("test source")]
        public void WithSource_WhenCalled_SetSource(string source)
        {            
            TemplateField field = new FakeTemplateField().WithSource(source);

            field.Source.Should().Be(source);
        }

        [Fact]
        public void FakeTemplateField_SetToolTip()
        {
            string fieldToolTip = "test tooltip";
            Language language = Language.Parse("da-DK");
            TemplateField field = new FakeTemplateField()
              .WithToolTip(fieldToolTip, language)
              ;

            field.GetToolTip(language).Should().Be(fieldToolTip);
            field.GetToolTip(Language.Parse("en")).Should().NotBe(fieldToolTip);
            field.GetToolTip(Language.Parse("en")).Should().Be("");
        }

        [AutoData]
        [Theory, InlineData("test link")]
        public void WithHelpLink_WhenCalled_SetHelpLink(string link)
        {            
            TemplateField field = new FakeTemplateField().WithHelpLink(link);

            field.HelpLink.Should().Be(link);
        }

        [AutoData]
        [Theory, InlineData(100)]
        public void FakeTemplateField_SetSortOrder(int sort)
        {             
            TemplateField field = new FakeTemplateField().WithSortorder(sort);

            field.Sortorder.Should().Be(sort);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithResetBlank_WhenCalled_SetResetBlank(bool reset)
        {             
            TemplateField field = new FakeTemplateField().WithResetBlank(reset);

            field.ResetBlank.Should().Be(reset);
        }

        [AutoData]
        [Theory, InlineData("validation")]
        public void WithValidation_WhenCalled_SetValidation(string validation)
        {
            TemplateField field = new FakeTemplateField().WithValidation(validation);

            field.Validation.Should().Be(validation);
        }

        [Fact]
        public void FakeTemplateField_SetDescription()
        {
            string fieldDescription = "test description";
            Language language = Language.Parse("da-DK");
            TemplateField field = new FakeTemplateField().WithDescription(fieldDescription, language);

            field.GetDescription(language).Should().Be(fieldDescription);
            field.GetDescription(Language.Parse("en")).Should().NotBe(fieldDescription);
            field.GetDescription(Language.Parse("en")).Should().Be("");
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithUnversioned_WhenCalled_SetUnversioned(bool unversioned)
        {
            TemplateField field = new FakeTemplateField().WithUnversioned(unversioned);

            field.IsUnversioned.Should().Be(unversioned);
        }

        [AutoData]
        [Theory, InlineData("default value")]
        public void WithDefaultValue_WhenCalled_SetsDefaultValue(string defaultValue)
        {            
            TemplateField field = new FakeTemplateField().WithDefaultValue(defaultValue);

            field.DefaultValue.Should().Be(defaultValue);
        }

        [Fact]
        public void FakeTemplateField_SetValidationText()
        {
            string text = "test validation text";
            Language language = Language.Parse("da-DK");
            TemplateField field = new FakeTemplateField()
              .WithValidationText(text, language)
              ;

            field.GetValidationText(language).Should().Be(text);
            field.GetValidationText(Language.Parse("en")).Should().NotBe(text);
            field.GetValidationText(Language.Parse("en")).Should().Be("");
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithExcludeFromTextSearch_WhenCalled_SetsExcludeFromTextSearch(bool configuredExcludeFromTextSearch)
        {            
            TemplateField field = new FakeTemplateField().WithExcludeFromTextSearch(configuredExcludeFromTextSearch);

            field.ExcludeFromTextSearch.Should().Be(configuredExcludeFromTextSearch);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithIgnoreDictionaryTranslations_WhenCalled_SetsIgnoreDictionaryTranslations(bool configuredIgnoreDictionaryTranslations)
        {
            TemplateField field = new FakeTemplateField().WithIgnoreDictionaryTranslations(configuredIgnoreDictionaryTranslations);

            field.IgnoreDictionaryTranslations.Should().Be(configuredIgnoreDictionaryTranslations);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void WithSharedLanguageFallbackEnabled_WhenCalled_SetsIsSharedLanguageFallbackEnabled(bool configuredIsSharedLanguageFallbackEnabled)
        {
            TemplateField field = new FakeTemplateField().WithSharedLanguageFallbackEnabled(configuredIsSharedLanguageFallbackEnabled);

            field.IsSharedLanguageFallbackEnabled.Should().Be(configuredIsSharedLanguageFallbackEnabled);
        }
    }
}
