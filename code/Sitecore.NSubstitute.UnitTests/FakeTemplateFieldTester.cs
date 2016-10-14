using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Globalization;

namespace Sitecore.NSubstituteUtils.UnitTests
{
  [TestFixture]
  public class FakeTemplateFieldTester
  {
    [Test]
    public void FakeTemplateField_DefaultInitialization()
    {
      var fakeField = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()));
      fakeField.Builder.Should().NotBeNull();
      var field = fakeField.ToSitecoreTemplateField();

      field.Should().NotBeNull();
      field.Name.Should().Be("fakeField");
      field.ID.Should().NotBeNull();
    }

    [Test]
    public void FakeTemplateField_NonDefaultInitialization()
    {
      string name = "test name";
      ID id = ID.NewID;
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()), name, id)
        .ToSitecoreTemplateField();

      field.Name.Should().Be(name);
      field.ID.Should().Be(id);
    }

    [Test]
    public void FakeTemplateField_SetIcon()
    {
      string icon = "test icon";
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithIcon(icon)
        .ToSitecoreTemplateField();

      field.Icon.Should().Be(icon);
    }

    [Test]
    public void FakeTemplateField_SetType()
    {
      string fieldType = "test type";
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithType(fieldType)
        .ToSitecoreTemplateField();

      field.Type.Should().Be(fieldType);
    }

    [Test]
    public void FakeTemplateField_SetStyle()
    {
      string fieldStyle = "test style";
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithStyle(fieldStyle)
        .ToSitecoreTemplateField();

      field.Style.Should().Be(fieldStyle);
    }

    [Test]
    public void FakeTemplateField_SetTitle()
    {
      string fieldTitle = "test title";
      Language language = Language.Parse("da-DK");
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithTitle(fieldTitle, language)
        .ToSitecoreTemplateField();

      field.GetTitle(language).Should().Be(fieldTitle);
      field.GetTitle(Language.Parse("en")).Should().NotBe(fieldTitle);
      field.GetTitle(Language.Parse("en")).Should().Be("");
    }

    [Test]
    public void FakeTemplateField_SetIsBlob()
    {
      bool isBlob = true;
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithIsBlob(isBlob)
        .ToSitecoreTemplateField();

      field.IsBlob.Should().Be(isBlob);
    }

    [Test]
    public void FakeTemplateField_SetIsShared()
    {
      bool isShared = true;
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithIsShared(isShared)
        .ToSitecoreTemplateField();

      field.IsShared.Should().Be(isShared);
    }

    [Test]
    public void FakeTemplateField_SetSource()
    {
      string source = "test source";
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithSource(source)
        .ToSitecoreTemplateField();

      field.Source.Should().Be(source);
    }

    [Test]
    public void FakeTemplateField_SetToolTip()
    {
      string fieldToolTip = "test tooltip";
      Language language = Language.Parse("da-DK");
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithToolTip(fieldToolTip, language)
        .ToSitecoreTemplateField();

      field.GetToolTip(language).Should().Be(fieldToolTip);
      field.GetToolTip(Language.Parse("en")).Should().NotBe(fieldToolTip);
      field.GetToolTip(Language.Parse("en")).Should().Be("");
    }

    [Test]
    public void FakeTemplateField_SetHelpLink()
    {
      string link = "test link";
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithHelpLink(link)
        .ToSitecoreTemplateField();

      field.HelpLink.Should().Be(link);
    }

    [Test]
    public void FakeTemplateField_SetSortorder()
    {
      int sort = 100;
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithSortorder(sort)
        .ToSitecoreTemplateField();

      field.Sortorder.Should().Be(sort);
    }

    [Test]
    public void FakeTemplateField_SetResetBlank()
    {
      bool reset = true;
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithResetBlank(reset)
        .ToSitecoreTemplateField();

      field.ResetBlank.Should().Be(reset);
    }

    [Test]
    public void FakeTemplateField_SetValidation()
    {
      string validation = "validation";
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithValidation(validation)
        .ToSitecoreTemplateField();

      field.Validation.Should().Be(validation);
    }

    [Test]
    public void FakeTemplateField_SetDescription()
    {
      string fieldDescription = "test description";
      Language language = Language.Parse("da-DK");
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithDescription(fieldDescription, language)
        .ToSitecoreTemplateField();

      field.GetDescription(language).Should().Be(fieldDescription);
      field.GetDescription(Language.Parse("en")).Should().NotBe(fieldDescription);
      field.GetDescription(Language.Parse("en")).Should().Be("");
    }

    [Test]
    public void FakeTemplateField_SetUnversioned()
    {
      bool unversioned = true;
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithUnversioned(unversioned)
        .ToSitecoreTemplateField();

      field.IsUnversioned.Should().Be(unversioned);
    }

    [Test]
    public void FakeTemplateField_SetDefaultValue()
    {
      string defaultValue = "default value";
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithDefaultValue(defaultValue)
        .ToSitecoreTemplateField();

      field.DefaultValue.Should().Be(defaultValue);
    }

    [Test]
    public void FakeTemplateField_SetValidationText()
    {
      string text = "test validation text";
      Language language = Language.Parse("da-DK");
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithValidationText(text, language)
        .ToSitecoreTemplateField();

      field.GetValidationText(language).Should().Be(text);
      field.GetValidationText(Language.Parse("en")).Should().NotBe(text);
      field.GetValidationText(Language.Parse("en")).Should().Be("");
    }

    [Test]
    public void FakeTemplateField_SetExcludeFromTextSearch()
    {
      bool exclude = true;
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithExcludeFromTextSearch(exclude)
        .ToSitecoreTemplateField();

      field.ExcludeFromTextSearch.Should().Be(exclude);
    }

    [Test]
    public void FakeTemplateField_SetIgnoreDictionaryTranslations()
    {
      bool ignore = true;
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithIgnoreDictionaryTranslations(ignore)
        .ToSitecoreTemplateField();

      field.IgnoreDictionaryTranslations.Should().Be(ignore);
    }

    [Test]
    public void FakeTemplateField_SetSharedLanguageFallbackEnabled()
    {
      bool enabled = true;
      var field = new FakeTemplateField(new FakeTemplateSection(new FakeTemplate()))
        .WithSharedLanguageFallbackEnabled(enabled)
        .ToSitecoreTemplateField();

      field.IsSharedLanguageFallbackEnabled.Should().Be(enabled);
    }
  }
}
