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
  }
}
