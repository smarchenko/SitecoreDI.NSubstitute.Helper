using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Templates;
using Sitecore.Globalization;

namespace Sitecore.NSubstituteUtils
{
  public class FakeTemplateField
  {
    private TemplateField.Builder builder;

    public FakeTemplateField(TemplateField.Builder builder)
    {
      this.builder = builder;
    }

    public FakeTemplateField(FakeTemplateSection templateSection = null, string fieldName = null, ID fieldId = null)
    {
      var name = fieldName ?? "fakeField";
      var id = fieldId ?? ID.NewID;
      var section = templateSection ?? new FakeTemplateSection();
      builder = new TemplateField.Builder(name, id, section);
    }

    public TemplateField.Builder Builder {
      get { return builder; }
    }

    public TemplateField ToSitecoreTemplateField()
    {
      return builder.Field;
    }

    public static implicit operator TemplateField(FakeTemplateField field)
    {
      return field.ToSitecoreTemplateField();
    }

    public FakeTemplateField WithIcon(string icon)
    {
      this.builder.SetIcon(icon);
      return this;
    }

    public FakeTemplateField WithType(string fieldType)
    {
      builder.SetType(fieldType);
      return this;
    }

    public FakeTemplateField WithStyle(string style)
    {
      builder.SetStyle(style);
      return this;
    }

    public FakeTemplateField WithTitle(string title, Language language)
    {
      builder.SetTitle(title, language);
      return this;
    }

    public FakeTemplateField WithIsBlob(bool isBlob)
    {
      builder.SetIsBlob(isBlob);
      return this;
    }

    public FakeTemplateField WithIsShared(bool isShared)
    {
      builder.SetShared(isShared);
      return this;
    }

    public FakeTemplateField WithSource(string source)
    {
      builder.SetSource(source);
      return this;
    }

    public FakeTemplateField WithToolTip(string toolTip, Language language)
    {
      builder.SetToolTip(toolTip, language);
      return this;
    }

    public FakeTemplateField WithHelpLink(string link)
    {
      builder.SetHelpLink(link);
      return this;
    }

    public FakeTemplateField WithSortorder(int sortorder)
    {
      builder.SetSortorder(sortorder);
      return this;
    }

    public FakeTemplateField WithResetBlank(bool resetBlank)
    {
      builder.SetResetBlank(resetBlank);
      return this;
    }

    public FakeTemplateField WithValidation(string validation)
    {
      builder.SetValidation(validation);
      return this;
    }

    public FakeTemplateField WithDescription(string description, Language language)
    {
      builder.SetDescription(description, language);
      return this;
    }

    public FakeTemplateField WithUnversioned(bool unversioned)
    {
      builder.SetUnversioned(unversioned);
      return this;
    }

    public FakeTemplateField WithDefaultValue(string value)
    {
      builder.SetDefaultValue(value);
      return this;
    }

    public FakeTemplateField WithValidationText(string text, Language language)
    {
      builder.SetValidationText(text, language);
      return this;
    }

    public FakeTemplateField WithExcludeFromTextSearch(bool exclude)
    {
      builder.SetExcludeFromTextSearch(exclude);
      return this;
    }

    public FakeTemplateField WithIgnoreDictionaryTranslations(bool ignore)
    {
      builder.SetIgnoreDictionaryTranslations(ignore);
      return this;
    }

    public FakeTemplateField WithSharedLanguageFallbackEnabled(bool enabled)
    {
      builder.SetSharedLanguageFallbackEnabled(enabled);
      return this;
    }
  }
}
