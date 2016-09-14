# SitecoreDI.NSubstitute.Helper
The reason for this project is to just show some hints to developers for how they can substitute some Sitecore classes.

If one builds the project using build scripts, he will be able to reference generated nuget package with helper utils for further using it in own project.

**Note**: the package does not reference Sitecore API nuget package so that it does not require Sitecore infrastructure. Please add references to Sitecore.Kernel file manually.

Example for creating a fake item object:
```c#
    [Test]
    public void FakeItem_ShouldReturn_FakeItemWithSpecifiedParameters()
    {
      var database = FakeUtil.FakeDatabase();
      var id = ID.NewID;
      var name = "test item name";

      var item = FakeUtil.FakeItem(id, name, database);

      item.Should().NotBeNull();
      item.ID.Should().Be(id);
      item.Name.Should().Be(name);
      item.Database.Should().Be(database);
    }
```


More examples for using helper methods you can find [here](https://github.com/smarchenko/SitecoreDI.NSubstitute.Helper/blob/master/code/Sitecore.NSubstitute.UnitTests/FakeUtilTester.cs)


The project also contains additional API that might be useful for testing the code, that has not been reworked, and still using staic Sitecore managegers.

**Note**: it is strongly recommended not to use this API and update the code to use DI instead. If this is not possible, please consider to extract usages of Siteocre statics to a separate virtual methods, so that they can be substituted in pure Unit Tests using proxy classes. 

If these optoions do not work for your solution, then you can try to use the FakeServiceProviderWrapper class that is designed to help in this case. However, you should remember that this solution must be temporary and code must be updated ASAP.

Code below substitutes the default implementation of BaseItemManger with an own one that will be grabbed by static ItemManager class:
```c#
    [Test]
    public void SimleProviderWrapper_ShouldWork()
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
```

**Note**: In case you find additional static managers that are not cleaned up by the wrapper, you can create own disposable wrapper and add your own cleanup logic. 