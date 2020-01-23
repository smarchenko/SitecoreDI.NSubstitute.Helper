# Sitecore.NSubstituteUtils
The reason for this project is to just show some hints to developers for how they can substitute some Sitecore classes.

If one builds the project using build scripts, he will be able to reference generated nuget package with helper utils for further using it in own project.

**Note**: the package does not reference Sitecore API nuget package so that it does not require Sitecore infrastructure. Please add references to Sitecore.Kernel file manually.

**Note** The solution is referencing some Sitecore assemblies taken from the [official public NuGet server](https://sitecore.myget.org/gallery/sc-packages)


## Creating Item and structures of items
FakeItem class allows to initialize the item with the most useful properties easily. 
This class also allows to create item structures in a few lines of code.

Example:
```C#
    [Test]
    public void FakeItem_ShouldSimplify_StructureCreation()
    {
      var item = new FakeItem();

      var parentID = ID.NewID;
      var childID1 = ID.NewID;
      var childID2 = ID.NewID;
      var scItem = (Item)item;
      
      // create fake item with specified parent and 2 children
      item
        .WithParent(new FakeItem(parentID))
        .WithChild(new FakeItem(childID1, scItem.Database))
        .WithChild(new FakeItem(childID2, scItem.Database));

      scItem.ParentID.Should().Be(parentID);
      scItem.Parent.ID.Should().Be(parentID);
      scItem.Children.Count.Should().Be(2);

      scItem.Database.GetItem(childID1).Should().NotBeNull();
      scItem.Database.GetItem(childID1).ID.Should().Be(childID1);
    }
```

Also the class can easily be extended by adding all necessary methods via extension methods.

Example:
```C#
  public static class TestFakeItemExtensions
  {
    public static FakeItem WithItemHelp(this FakeItem item, ItemHelp itemHelp)
    {
      item.ToSitecoreItem().Help.Returns(itemHelp);

      return item;
    }
  }
```

More examples your can find [here](https://github.com/smarchenko/SitecoreDI.NSubstitute.Helper/blob/master/code/Sitecore.NSubstitute.UnitTests/FakeItemTester.cs)

## Creating SiteContext
Starting from version 2.0.2 you can use `SiteInfoPropertiesBuilder` to create site properties and later on cast to one of the Sitecore types, like `SiteInfo`, `Site`, `SiteContext`:
```C#
        [Test]
        public void CreatingSiteContext_ShouldBeTrivial()
        {
            SiteContext siteContext = new SiteInfoPropertiesBuilder("TestSiteName")
                .WithHostName("test-site-host")
                .WithDatabase("test-database")
                .WithStartItem("/test/start/items");

            siteContext.StartItem.Should().Be("/test/start/items");
        }
```


## Simple substitutes
FakeUtil class has a number of simple methods that show how to fake some Sitecore classes or parts of the Item class. This class has been created just for demo purpose. I would recommend to use FakeItem class in your test projects since it is much more powerful one and simplifies creation of item structures. 

Example for creating a fake item object:
```C#
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

## Unittesting with static managers  
The project also contains additional API that might be useful for testing the code, that has not been reworked, and still using static Sitecore managers.

**Note**: it is strongly recommended not to use this API and update the code to use DI instead. If this is not possible, please consider to extract usages of Sitecore statics to a separate virtual methods, so that they can be substituted in pure Unit Tests using proxy classes. 

If these options do not work for your solution, then you can try to use the FakeServiceProviderWrapper class that is designed to help in this case. However, you should remember that this solution must be temporary and code must be updated ASAP.

Code below substitutes the default implementation of BaseItemManger with an own one that will be grabbed by static ItemManager class:
```C#
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
