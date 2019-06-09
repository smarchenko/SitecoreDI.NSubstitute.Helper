using Sitecore.Collections;
using Sitecore.Sites;
using Sitecore.Web;

namespace Sitecore.NSubstituteUtils.Extensions
{
    public static class SiteInfoPropertiesExtensions
    {
        public static SiteInfo ToSiteInfo(this SiteInfoPropertiesBuilder builder)
        {
            if (builder == null)
            {
                return null;
            }

            return new SiteInfo(builder.ToSitecoreSiteInfoProperties());
        }

        public static Site ToSite(this SiteInfoPropertiesBuilder builder)
        {
            if (builder == null)
            {
                return null;
            }

            StringDictionary properties = builder.ToSitecoreSiteInfoProperties();
            return new Site(properties[Constants.SiteProperties.SiteName], properties);
        }

        public static SiteContext ToSiteContext(this SiteInfoPropertiesBuilder builder)
        {
            if (builder == null)
            {
                return null;
            }

            return new SiteContext(builder.ToSiteInfo());
        }
    }
}
