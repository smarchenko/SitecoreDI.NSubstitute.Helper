using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.ItemResolvers;
using Sitecore.Data.Templates;

namespace Sitecore.NSubstituteUtils
{
  public class FakeTemplateSection
  {
    private TemplateSection.Builder builder;

    public FakeTemplateSection(TemplateSection.Builder builder)
    {
      this.builder = builder;
    }

    public FakeTemplateSection(FakeTemplate template = null, string sectionName = null, ID sectionId = null)
    {
      var name = sectionName ?? "fakeSection";
      var id = sectionId ?? ID.NewID;
      var fakeTemplate = template ?? new FakeTemplate();
      builder = new TemplateSection.Builder(name, id, fakeTemplate);
    }

    public FakeTemplateField AddField(string name, ID id)
    {
      var fieldBuilder = builder.AddField(name, id);
      return new FakeTemplateField(fieldBuilder);
    }
    
    public TemplateSection ToSitecoreTemplateSection()
    {
      return this.builder.Section; 
    }

    public TemplateSection.Builder Builder
    {
      get { return this.builder; }
    }

    public static implicit operator TemplateSection(FakeTemplateSection section)
    {
      return section.ToSitecoreTemplateSection();
    }

    public FakeTemplateSection WithIcon(string icon)
    {
      this.builder.SetIcon(icon);
      return this;
    }

    public FakeTemplateSection WithSortorder(int sortorder)
    {
      this.builder.SetSortorder(sortorder);
      return this;
    }
  }
}
