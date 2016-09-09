using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.Data;

namespace Sitecore.NSubstitute.UnitTests
{
  [TestFixture]
  public class FakeUtilTester
  {
    [Test]
    public void FakeDatabase_ShouldReturn_FakeDatabaseWithSpecifiedName()
    {
      string databaseName = "fake name";
      var database = FakeUtil.FakeDatabase(databaseName);

      database.Should().NotBeNull();
      database.Should().BeAssignableTo<Database>();
      database.Name.Should().Be(databaseName);
    }

    [Test]
    public void FakeDatabase_ShouldReturn_FakeDatabaseWithDefaultName()
    {
      var database = FakeUtil.FakeDatabase();

      database.Should().NotBeNull();
      database.Should().BeAssignableTo<Database>();
      database.Name.Should().Be("fakeDB");
    }
  }
}
