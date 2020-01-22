using System;
using FluentAssertions;
using Sitecore.Data;
using Sitecore.NSubstituteUtils;
using Xunit;

namespace Sitecore.NSubstitute.UnitTests
{
    public class SiteInfoPropertiesBuilderTester
    {
        [Fact]
        public void EmptySiteInfoProperties_CanBeCreated()
        {
            new SiteInfoPropertiesBuilder().ToSitecoreSiteInfoProperties().Should().NotBeNull();
        }

        [Fact]
        public void EmptySiteInfoProperties_CanBeCreatedWithSpecifiedName()
        {
            string siteName = Guid.NewGuid().ToString();
            var properties = new SiteInfoPropertiesBuilder(siteName).ToSitecoreSiteInfoProperties();

            properties[NSubstituteUtils.Constants.SiteProperties.SiteName].Should().Be(siteName);
        }

        [Fact]
        public void ToSitecoreSiteInfoProperties_ReturnsCopyWhenAsked()
        {
            string siteName = Guid.NewGuid().ToString();
            string newSiteName = Guid.NewGuid().ToString();
            var builder = new SiteInfoPropertiesBuilder(siteName);
            var properties = builder.ToSitecoreSiteInfoProperties(true);
            var newProperties = builder.WithSiteName(newSiteName).ToSitecoreSiteInfoProperties();

            properties[NSubstituteUtils.Constants.SiteProperties.SiteName].Should().Be(siteName);
            newProperties[NSubstituteUtils.Constants.SiteProperties.SiteName].Should().Be(newSiteName);
        }

        [Fact]
        public void ToSitecoreSiteInfoProperties_ReturnsInnerCollectionByDefault()
        {
            string siteName = Guid.NewGuid().ToString();
            string newSiteName = Guid.NewGuid().ToString();
            var builder = new SiteInfoPropertiesBuilder(siteName);
            var properties = builder.ToSitecoreSiteInfoProperties();
            builder.WithSiteName(newSiteName);

            properties[NSubstituteUtils.Constants.SiteProperties.SiteName].Should().Be(newSiteName);
        }

        [Fact]
        public void SiteInfoProperties_InitializedCorrectly()
        {
            string siteName = new ShortID().ToString();
            string hostName = new ShortID().ToString();
            string virtualFolder = new ShortID().ToString();
            string physicalFolder = new ShortID().ToString();
            string rootPath = new ShortID().ToString();
            string siteDefinitionPath = new ShortID().ToString();
            string startItem = new ShortID().ToString();
            string database = new ShortID().ToString();
            string domain = new ShortID().ToString();
            string language = new ShortID().ToString();
            string targetHostName = new ShortID().ToString();
            bool allowDebug = true;
            bool cacheHtml = true;
            bool enablePreview = true;
            bool enableWebEdit = true;
            bool enableDebugger = true;
            bool disableClientData = true;
            bool languageEmbedding = true;
            string htmlCacheSize = new ShortID().ToString();
            string registryCacheSize = new ShortID().ToString();
            string viewStateCacheSize = new ShortID().ToString();
            string xslCacheSize = new ShortID().ToString();
            string filteredItemsCacheSize = new ShortID().ToString();
            bool disableBrowserCaching = true;
            string loginPage = new ShortID().ToString();
            string provider = new ShortID().ToString();
            bool requireLogin = true;

            var properties = new SiteInfoPropertiesBuilder()
                .WithSiteName(siteName)
                .WithHostName(hostName)
                .WithVirtualFolder(virtualFolder)
                .WithPhysicalFolder(physicalFolder)
                .WithRootPath(rootPath)
                .WithSiteDefinitionPath(siteDefinitionPath)
                .WithStartItem(startItem)
                .WithDatabase(database)
                .WithDomain(domain)
                .WithLanguage(language)
                .WithTargetHostName(targetHostName)
                .WithAllowDebug(allowDebug)
                .WithCacheHtml(cacheHtml)
                .WithEnablePreview(enablePreview)
                .WithEnableWebEdit(enableWebEdit)
                .WithEnableDebugger(enableDebugger)
                .WithDisableClientData(disableClientData)
                .WithLanguageEmbedding(languageEmbedding)
                .WithHtmlCacheSize(htmlCacheSize)
                .WithRegistryCacheSize(registryCacheSize)
                .WithViewStateCacheSize(viewStateCacheSize)
                .WithXslCacheSize(xslCacheSize)
                .WithFilteredItemsCacheSize(filteredItemsCacheSize)
                .WithDisableBrowserCaching(disableBrowserCaching)
                .WithLoginPage(loginPage)
                .WithProvider(provider)
                .WithRequireLogin(requireLogin)
                .ToSitecoreSiteInfoProperties();

            properties[NSubstituteUtils.Constants.SiteProperties.SiteName].Should().Be(siteName);
            properties[NSubstituteUtils.Constants.SiteProperties.HostName].Should().Be(hostName);
            properties[NSubstituteUtils.Constants.SiteProperties.VirtualFolder].Should().Be(virtualFolder);
            properties[NSubstituteUtils.Constants.SiteProperties.PhysicalFolder].Should().Be(physicalFolder);
            properties[NSubstituteUtils.Constants.SiteProperties.RootPath].Should().Be(rootPath);
            properties[NSubstituteUtils.Constants.SiteProperties.SiteDefinitionPath].Should().Be(siteDefinitionPath);
            properties[NSubstituteUtils.Constants.SiteProperties.StartItem].Should().Be(startItem);
            properties[NSubstituteUtils.Constants.SiteProperties.Database].Should().Be(database);
            properties[NSubstituteUtils.Constants.SiteProperties.Domain].Should().Be(domain);
            properties[NSubstituteUtils.Constants.SiteProperties.Language].Should().Be(language);
            properties[NSubstituteUtils.Constants.SiteProperties.TargetHostName].Should().Be(targetHostName);
            properties[NSubstituteUtils.Constants.SiteProperties.AllowDebug].Should().Be(allowDebug.ToString().ToLowerInvariant());
            properties[NSubstituteUtils.Constants.SiteProperties.CacheHtml].Should().Be(cacheHtml.ToString().ToLowerInvariant());
            properties[NSubstituteUtils.Constants.SiteProperties.EnablePreview].Should().Be(enablePreview.ToString().ToLowerInvariant());
            properties[NSubstituteUtils.Constants.SiteProperties.EnableWebEdit].Should().Be(enableWebEdit.ToString().ToLowerInvariant());
            properties[NSubstituteUtils.Constants.SiteProperties.EnableDebugger].Should().Be(enableDebugger.ToString().ToLowerInvariant());
            properties[NSubstituteUtils.Constants.SiteProperties.DisableClientData].Should().Be(disableClientData.ToString().ToLowerInvariant());
            properties[NSubstituteUtils.Constants.SiteProperties.LanguageEmbedding].Should().Be(languageEmbedding.ToString().ToLowerInvariant());
            properties[NSubstituteUtils.Constants.SiteProperties.HtmlCacheSize].Should().Be(htmlCacheSize);
            properties[NSubstituteUtils.Constants.SiteProperties.RegistryCacheSize].Should().Be(registryCacheSize);
            properties[NSubstituteUtils.Constants.SiteProperties.ViewStateCacheSize].Should().Be(viewStateCacheSize);
            properties[NSubstituteUtils.Constants.SiteProperties.XslCacheSize].Should().Be(xslCacheSize);
            properties[NSubstituteUtils.Constants.SiteProperties.FilteredItemsCacheSize].Should().Be(filteredItemsCacheSize);
            properties[NSubstituteUtils.Constants.SiteProperties.DisableBrowserCaching].Should().Be(disableBrowserCaching.ToString().ToLowerInvariant());
            properties[NSubstituteUtils.Constants.SiteProperties.LoginPage].Should().Be(loginPage);
            properties[NSubstituteUtils.Constants.SiteProperties.Provider].Should().Be(provider);
            properties[NSubstituteUtils.Constants.SiteProperties.RequireLogin].Should().Be(requireLogin.ToString().ToLowerInvariant());
        }
    }
}
