using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.Data;


namespace Sitecore.NSubstituteUtils.UnitTests
{
  [TestFixture]
  public class FakeTemplateTester
  {
    [Test]
    public void FakeTemplate_ParametrlessInitialization()
    {
      var template = new FakeTemplate().ToSitecoreTemplate();

      template.Should().NotBeNull();
      template.ID.Should().NotBeNull();
      template.Name.Should().Be("fakeTemplate");
      template.BaseIDs.Should().NotBeNull();
      template.BaseIDs.Length.Should().Be(0);
    }

    [Test]
    public void FakeTemplate_ParametrizedInitialization()
    {
      ID id = ID.NewID;
      string name = "test";
      var template = new FakeTemplate(name, id).ToSitecoreTemplate();

      template.Name.Should().Be(name);
      template.ID.Should().Be(id);
    }

    [Test]
    public void FakeTemplate_BaseIDs()
    {
      string baseIDs = ID.NewID.ToString();
      var template = new FakeTemplate()
        .WithBaseIDs(new ID[] {new ID(baseIDs)}).ToSitecoreTemplate();

      template.BaseIDs.Length.Should().Be(1);
      template.BaseIDs[0].Should().Be(new ID(baseIDs));
      template.GetBaseTemplates().Should().NotBeNull();
      template.GetBaseTemplates()[0].ID.Should().Be(new ID(baseIDs));
    }
  }
}
