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


    public FakeTemplate(string templateName = null, ID templateId = null, Database database = null)
    {
      string name = string.IsNullOrEmpty(templateName) ? "fakeTemplate" : templateName;
      ID id = templateId ?? ID.NewID;
      Database db = database ?? FakeUtil.FakeDatabase();
      this.Database = db;

      templateEngine = Substitute.For<TemplateEngine>(db);
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
  }
}
