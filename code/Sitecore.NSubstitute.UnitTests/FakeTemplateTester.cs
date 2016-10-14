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

    [Test]
    public void FakeTemplate_Descendants()
    {
      string baseIDs = ID.NewID.ToString();
      var template = new FakeTemplate()
        .WithBaseIDs(new ID[] { new ID(baseIDs) }).ToSitecoreTemplate();

      template.DescendsFrom(new ID(baseIDs)).Should().BeTrue();
      template.DescendsFrom(ID.NewID).Should().BeFalse();
      template.DescendsFromOrEquals(new ID(baseIDs)).Should().BeTrue();
      template.DescendsFromOrEquals(ID.NewID).Should().BeFalse();
      template.DescendsFromOrEquals(template.ID).Should().BeTrue();
    }

    [Test]
    public void FakeTemplate_SetFullName()
    {
      string fullName = "fake full name";
      var template = new FakeTemplate()
        .WithFullName(fullName);

      template.ToSitecoreTemplate().FullName.Should().Be(fullName);
      template.TemplateEngine.GetTemplate(fullName).Should().Be(template.ToSitecoreTemplate());
    }

    [Test]
    public void FakeTemplate_SetIcon()
    {
      string icon = "some fake icon";
      var template = new FakeTemplate()
        .WithIcon(icon).ToSitecoreTemplate();

      template.Icon.Should().Be(icon);
    }

    [Test]
    public void FakeTemplate_SetStandardValues()
    {
      ID svId = ID.NewID;
      var template = new FakeTemplate().WithStandatdValues(svId);

      template.ToSitecoreTemplate().StandardValueHolderId.Should().Be(svId);
    }

    [Test]
    public void FakeTemplate_AddSection()
    {
      string name = "test name";
      ID id = ID.NewID;

      var fakeTemplate = new FakeTemplate();
      var section = fakeTemplate.AddSection(name, id);

      fakeTemplate.ToSitecoreTemplate().GetSections().Length.Should().Be(1);
      fakeTemplate.ToSitecoreTemplate().GetSection(id).Should().Be(section.ToSitecoreTemplateSection());
    }

    [Test]
    public void FakeTemplate_AddField()
    {
      string name1 = "name1";
      string name2 = "name2";
      string name3 = "name3";
      string sectionName1 = "section1";
      string sectionName2 = "section2";

      var template = new FakeTemplate();
      template.AddField(sectionName1, name1, ID.NewID);
      template.AddField(sectionName1, name2, ID.NewID);
      template.AddField(sectionName2, name3, ID.NewID);

      template.ToSitecoreTemplate().GetSections().Length.Should().Be(2);
      template.ToSitecoreTemplate().GetSection(sectionName1).GetFields().Length.Should().Be(2);
      template.ToSitecoreTemplate().GetSection(sectionName2).GetFields().Length.Should().Be(1);

      template.ToSitecoreTemplate().GetFields().Length.Should().Be(3);
      template.ToSitecoreTemplate()
        .GetField(name2)
        .Section.Should()
        .Be(template.ToSitecoreTemplate().GetSection(sectionName1));
    }
  }
}
