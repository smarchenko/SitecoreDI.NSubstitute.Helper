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
  }
}
