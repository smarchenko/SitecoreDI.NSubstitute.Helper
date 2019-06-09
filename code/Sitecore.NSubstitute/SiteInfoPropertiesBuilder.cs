using Sitecore.Data;
using StringDictionary = Sitecore.Collections.StringDictionary;

namespace Sitecore.NSubstituteUtils
{
    public class SiteInfoPropertiesBuilder
    {
        public SiteInfoPropertiesBuilder()
        {
            SiteInfoProperties = new StringDictionary();
            WithSiteName(new ShortID().ToString());
        }

        protected StringDictionary SiteInfoProperties { get; }

        public SiteInfoPropertiesBuilder WithSiteName(string siteName)
        {
            SiteInfoProperties[Constants.SiteProperties.SiteName] = siteName;
            return this;
        }

        public SiteInfoPropertiesBuilder WithHostName(string hostName)
        {
            SiteInfoProperties[Constants.SiteProperties.HostName] = hostName;
            return this;
        }

        public SiteInfoPropertiesBuilder WithVirtualFolder(string virtualFolder)
        {
            SiteInfoProperties[Constants.SiteProperties.VirtualFolder] = virtualFolder;
            return this;
        }

        public SiteInfoPropertiesBuilder WithPhysicalFolder(string physicalFolder)
        {
            SiteInfoProperties[Constants.SiteProperties.PhysicalFolder] = physicalFolder;
            return this;
        }

        public SiteInfoPropertiesBuilder WithRootPath(string rootPath)
        {
            SiteInfoProperties[Constants.SiteProperties.RootPath] = rootPath;
            return this;
        }

        public SiteInfoPropertiesBuilder WithSiteDefinitionPath(string siteDefinitionPath)
        {
            SiteInfoProperties[Constants.SiteProperties.SiteDefinitionPath] = siteDefinitionPath;
            return this;
        }

        public SiteInfoPropertiesBuilder WithStartItem(string startItem)
        {
            SiteInfoProperties[Constants.SiteProperties.StartItem] = startItem;
            return this;
        }

        public SiteInfoPropertiesBuilder WithDatabase(string database)
        {
            SiteInfoProperties[Constants.SiteProperties.Database] = database;
            return this;
        }

        public SiteInfoPropertiesBuilder WithDomain(string domain)
        {
            SiteInfoProperties[Constants.SiteProperties.Domain] = domain;
            return this;
        }

        public SiteInfoPropertiesBuilder WithLanguage(string language)
        {
            SiteInfoProperties[Constants.SiteProperties.Language] = language;
            return this;
        }

        public SiteInfoPropertiesBuilder WithTargetHostName(string targetHostName)
        {
            SiteInfoProperties[Constants.SiteProperties.TargetHostName] = targetHostName;
            return this;
        }

        public SiteInfoPropertiesBuilder WithAllowDebug(bool allowDebug)
        {
            SiteInfoProperties[Constants.SiteProperties.AllowDebug] = allowDebug.ToString().ToLowerInvariant();
            return this;
        }

        public SiteInfoPropertiesBuilder WithCacheHtml(bool cacheHtml)
        {
            SiteInfoProperties[Constants.SiteProperties.CacheHtml] = cacheHtml.ToString().ToLowerInvariant();
            return this;
        }

        public SiteInfoPropertiesBuilder WithEnablePreview(bool enablePreview)
        {
            SiteInfoProperties[Constants.SiteProperties.EnablePreview] = enablePreview.ToString().ToLowerInvariant();
            return this;
        }

        public SiteInfoPropertiesBuilder WithEnableWebEdit(bool enableWebEdit)
        {
            SiteInfoProperties[Constants.SiteProperties.EnableWebEdit] = enableWebEdit.ToString().ToLowerInvariant();
            return this;
        }

        public SiteInfoPropertiesBuilder WithEnableDebugger(bool enableDebugger)
        {
            SiteInfoProperties[Constants.SiteProperties.EnableDebugger] = enableDebugger.ToString().ToLowerInvariant();
            return this;
        }

        public SiteInfoPropertiesBuilder WithDisableClientData(bool disableClientData)
        {
            SiteInfoProperties[Constants.SiteProperties.DisableClientData] =
                disableClientData.ToString().ToLowerInvariant();
            return this;
        }

        public SiteInfoPropertiesBuilder WithLanguageEmbedding(bool languageEmbedding)
        {
            SiteInfoProperties[Constants.SiteProperties.LanguageEmbedding] =
                languageEmbedding.ToString().ToLowerInvariant();
            return this;
        }

        public SiteInfoPropertiesBuilder WithHtmlCacheSize(string htmlCacheSize)
        {
            SiteInfoProperties[Constants.SiteProperties.HtmlCacheSize] = htmlCacheSize;
            return this;
        }

        public SiteInfoPropertiesBuilder WithRegistryCacheSize(string registryCacheSize)
        {
            SiteInfoProperties[Constants.SiteProperties.RegistryCacheSize] = registryCacheSize;
            return this;
        }

        public SiteInfoPropertiesBuilder WithViewStateCacheSize(string viewStateCacheSize)
        {
            SiteInfoProperties[Constants.SiteProperties.ViewStateCacheSize] = viewStateCacheSize;
            return this;
        }

        public SiteInfoPropertiesBuilder WithXslCacheSize(string xslCacheSize)
        {
            SiteInfoProperties[Constants.SiteProperties.XslCacheSize] = xslCacheSize;
            return this;
        }

        public SiteInfoPropertiesBuilder WithFilteredItemsCacheSize(string filteredItemsCacheSize)
        {
            SiteInfoProperties[Constants.SiteProperties.FilteredItemsCacheSize] = filteredItemsCacheSize;
            return this;
        }

        public SiteInfoPropertiesBuilder WithDisableBrowserCaching(bool disableBrowserCaching)
        {
            SiteInfoProperties[Constants.SiteProperties.DisableBrowserCaching] =
                disableBrowserCaching.ToString().ToLowerInvariant();
            return this;
        }

        public SiteInfoPropertiesBuilder WithLoginPage(string loginPage)
        {
            SiteInfoProperties[Constants.SiteProperties.LoginPage] = loginPage;
            return this;
        }

        public SiteInfoPropertiesBuilder WithProvider(string provider)
        {
            SiteInfoProperties[Constants.SiteProperties.Provider] = provider;
            return this;
        }

        public SiteInfoPropertiesBuilder WithRequireLogin(bool requireLogin)
        {
            SiteInfoProperties[Constants.SiteProperties.RequireLogin] = requireLogin.ToString().ToLowerInvariant();
            return this;
        }

        public StringDictionary ToSitecoreSiteInfoProperties()
        {
            return SiteInfoProperties;
        }
    }
}
