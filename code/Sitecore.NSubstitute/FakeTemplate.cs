using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Sitecore.Abstractions;
using Sitecore.Data;
using Sitecore.Data.Engines;
using Sitecore.Data.Templates;
using Sitecore.Pipelines.HttpRequest;

namespace Sitecore.NSubstituteUtils
{
  public class FakeTemplate
  {
    private Template.Builder templateBuilder;
    private TemplateEngine templateEngine;

    private List<FakeTemplateSection> sections = new List<FakeTemplateSection>();


    public FakeTemplate(string templateName = null, ID templateId = null, Database database = null, TemplateEngine engine = null)
    {
      string name = string.IsNullOrEmpty(templateName) ? "fakeTemplate" : templateName;
      ID id = templateId ?? ID.NewID;
      Database db = database ?? FakeUtil.FakeDatabase();

      this.Database = db;

      templateEngine = engine ?? Substitute.For<TemplateEngine>(db);
      templateBuilder = new Template.Builder(name, id, templateEngine);
      templateBuilder.SetName(name);
      templateBuilder.SetID(id);

      templateEngine.GetTemplate(id).Returns(this);
      templateEngine.GetTemplate(name).Returns(this);
    }

    public Database Database { get; private set; }

    public TemplateEngine TemplateEngine
    {
      get { return this.templateEngine; }
    }

    public Template.Builder TemplateBuilder
    {
      get { return this.templateBuilder; }
    }

    public FakeTemplate WithStandatdValues(ID standardValuesHolderId)
    {
      templateBuilder.SetStandardValueHolderId(standardValuesHolderId.ToString());
      return this;
    }

    public FakeTemplate WithIcon(string icon)
    {
      templateBuilder.SetIcon(icon);
      return this;
    }

    public FakeTemplate WithFullName(string fullName)
    {
      templateBuilder.SetFullName(fullName);
      templateEngine.GetTemplate(fullName).Returns(this);
      return this;
    }

    public FakeTemplate WithBaseIDs(ID[] baseIDs)
    {
      if (baseIDs == null || baseIDs.Length == 0)
      {
        return this;
      }

      StringBuilder ids = new StringBuilder();
      for(int i = 0; i < baseIDs.Length; i++)
      {
        if (i > 0)
        {
          ids.Append("|");
        }

        ids.Append(baseIDs[i].ToString());
        string name = "Template_" + baseIDs[i];
        var template = new FakeTemplate(name, baseIDs[i], this.Database);
        templateEngine.GetTemplate(baseIDs[i]).Returns(template);
        templateEngine.GetTemplate(name).Returns(template);
      }

      templateBuilder.SetBaseIDs(ids.ToString());
      return this;
    }

    private Template Template
    {
      get { return this.templateBuilder.Template; }
    }

    public static implicit operator Template(FakeTemplate fakeTemplate)
    {
      return fakeTemplate.Template;
    }

    public Template ToSitecoreTemplate()
    {
      return Template;
    }

    public FakeTemplateSection AddSection(string name, ID id)
    {
      var builder = templateBuilder.AddSection(name, id);
      var result  = new FakeTemplateSection(builder);
      sections.Add(result);
      return result;
    }

    public FakeTemplateField AddField(string fieldName, ID fieldId)
    {
      return AddField(ID.NewID, fieldName, fieldId);
    }

    public FakeTemplateField AddField(ID sectionId, string fieldName, ID fieldId)
    {
      var sectionBuilder = this.sections.FirstOrDefault(s => s.ToSitecoreTemplateSection().ID == sectionId);
      if (sectionBuilder == null)
      {
        sectionBuilder = this.AddSection(sectionId.ToString(), sectionId);
      }

      return sectionBuilder.AddField(fieldName, fieldId);
    }

    public FakeTemplateField AddField(string sectionName, string fieldName, ID fieldId)
    {
      var sectionBuilder = this.sections.FirstOrDefault(s => s.ToSitecoreTemplateSection().Name.Equals(sectionName, StringComparison.InvariantCultureIgnoreCase));
      if (sectionBuilder == null)
      {
        sectionBuilder = this.AddSection(sectionName, ID.NewID);
      }

      return sectionBuilder.AddField(fieldName, fieldId);
    }

  }
}
