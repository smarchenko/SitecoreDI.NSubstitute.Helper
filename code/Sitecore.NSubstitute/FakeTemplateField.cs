using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Templates;

namespace Sitecore.NSubstituteUtils
{
  public class FakeTemplateField
  {
    private TemplateField.Builder builder;
    private FakeTemplateSection section;

    public FakeTemplateField(FakeTemplateSection templateSection, string fieldName = null, ID fieldId = null)
    {
      var name = fieldName ?? "fakeField";
      var id = fieldId ?? ID.NewID;
      this.section = templateSection;
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
  }
}
