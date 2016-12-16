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
  public class FakeSectionTester
  {
    [Test]
    public void FakeSection_DefaultInitialization()
    {
      var template = new FakeTemplate();
      var section = new FakeTemplateSection(template).ToSitecoreTemplateSection();
      section.Should().NotBeNull();
      section.ID.Should().NotBeNull();
      section.Template.Should().Be(template.ToSitecoreTemplate());
      section.Name.Should().Be("fakeSection");
    }

    [Test]
    public void FakeSection_EmptyContructor()
    {
      var section = new FakeTemplateSection().ToSitecoreTemplateSection();

      section.Should().NotBeNull();
    }

    [Test]
    public void FakeSection_NonDefaultInitialization()
    {
      string name = "test name";
      ID id = ID.NewID;

      var section = new FakeTemplateSection(new FakeTemplate(), name, id).ToSitecoreTemplateSection();
      
      section.ID.Should().Be(id);
      section.Name.Should().Be(name);
    }

    [Test]
    public void FakeSection_SetIcon()
    {
      string icon = "test icon";
      var section = new FakeTemplateSection(new FakeTemplate())
        .WithIcon(icon)
        .ToSitecoreTemplateSection();

      section.Icon.Should().Be(icon);
    }

    [Test]
    public void FakeSection_SetSortorder()
    {
      int sortorder = 42;
      var section = new FakeTemplateSection(new FakeTemplate())
        .WithSortorder(sortorder)
        .ToSitecoreTemplateSection();

      section.Sortorder.Should().Be(sortorder);
    }

    [Test]
    public void FakeSection_DefaultGetFields()
    {
      var section = new FakeTemplateSection(new FakeTemplate()).ToSitecoreTemplateSection();

      section.GetFields().Should().NotBeNull();
      section.GetFields().Length.Should().Be(0);
    }

    [Test]
    public void FakeSection_AddField()
    {
      string name = "test name";
      ID id = ID.NewID;

      var section = new FakeTemplateSection(new FakeTemplate());
      var field = section.AddField(name, id);
      field.WithSortorder(42);

      field.Should().NotBeNull();
      field.ToSitecoreTemplateField().ID.Should().Be(id);

      section.ToSitecoreTemplateSection().GetFields().Length.Should().Be(1);
      section.ToSitecoreTemplateSection().GetFields()[0].ID.Should().Be(id);
      
      section.ToSitecoreTemplateSection().GetField(id).Should().Be(field.ToSitecoreTemplateField());
      section.ToSitecoreTemplateSection().GetField(name).Should().Be(field.ToSitecoreTemplateField());

      section.ToSitecoreTemplateSection().GetField(id).Sortorder.Should().Be(42);
    }
  }
}
