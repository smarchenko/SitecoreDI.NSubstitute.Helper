using System;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.Data.Managers;

namespace Sitecore.NSubstituteUtils.UnitTests
{
 
  [Trait("Unit", "ServiceLocator-related")]
  public class FakeServiceProviderTester
  {
    [Fact]
    public void SimpleProviderWrapper_ShouldWork()
    {
      var item = FakeUtil.FakeItem();
      var service1 = Substitute.For<BaseItemManager>();
      service1.GetParent(item).Returns(item);

      var fakeServiceProvider = Substitute.For<IServiceProvider>();
      fakeServiceProvider.GetService(typeof(BaseItemManager)).Returns(service1);
      fakeServiceProvider.GetService(
        typeof(ProviderHelper<ItemProviderBase, ItemProviderCollection>))
        .Returns(
          Substitute.For<ProviderHelper<ItemProviderBase, ItemProviderCollection>>("/somepath"));

      using (new FakeServiceProviderWrapper(fakeServiceProvider))
      {
        ItemManager.GetParent(item).Returns(item);
      }
    }

    [Fact]
    public void ShouldResolveServiceUsingLatestRegisteredServiceProvider()
    {
      var item = FakeUtil.FakeItem();
      var service1 = Substitute.For<BaseItemManager>();
      var tmpItem1 = FakeUtil.FakeItem();
      service1.GetParent(item).Returns(tmpItem1);

      var service2 = Substitute.For<BaseItemManager>();
      var tmpItem2 = FakeUtil.FakeItem();
      service2.GetParent(item).Returns(tmpItem2);

      service1.GetParent(item).Should().NotBe(service2.GetParent(item));

      Assert.Throws<TypeInitializationException>(() =>
      {
        var a = Sitecore.Data.Managers.ItemManager.GetParent(item);
      });

      
      var fakeServiceProvider = Substitute.For<IServiceProvider>();
      fakeServiceProvider.GetService(typeof(BaseItemManager)).Returns(service1);
      fakeServiceProvider.GetService(
        typeof(ProviderHelper<ItemProviderBase, ItemProviderCollection>))
        .Returns(
          Substitute.For<ProviderHelper<ItemProviderBase, ItemProviderCollection>>("/somepath"));

      using (new FakeServiceProviderWrapper(fakeServiceProvider))
      {
        Sitecore.Data.Managers.ItemManager.GetParent(item).Returns(tmpItem1);
      }

      Assert.Throws<TypeInitializationException>(() =>
      {
        var a = Sitecore.Data.Managers.ItemManager.GetParent(item);
      });

      using (new FakeServiceProviderWrapper(fakeServiceProvider))
      {
        Sitecore.Data.Managers.ItemManager.GetParent(item).Returns(tmpItem1);
      }

      fakeServiceProvider.GetService(typeof(BaseItemManager)).Returns(service2);
      using (new FakeServiceProviderWrapper(fakeServiceProvider))
      {
        Sitecore.Data.Managers.ItemManager.GetParent(item).Returns(tmpItem2);
      }

      Assert.Throws<TypeInitializationException>(() =>
      {
        var a = Sitecore.Data.Managers.ItemManager.GetParent(item);
      });
    }
  }
}
