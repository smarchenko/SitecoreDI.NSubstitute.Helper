using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Caching;
using Sitecore.Caching.Placeholders;
using Sitecore.CodeDom.Scripts;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Archiving;
using Sitecore.Data.Comparers;
using Sitecore.Data.Fields;
using Sitecore.Data.LanguageFallback;
using Sitecore.Data.Managers;
using Sitecore.Data.Proxies;
using Sitecore.Data.Serialization;
using Sitecore.Data.Validators;
using Sitecore.Diagnostics;
using Sitecore.Eventing;
using Sitecore.Globalization;
using Sitecore.Jobs;
using Sitecore.Links;
using Sitecore.Pipelines;
using Sitecore.Presentation;
using Sitecore.Publishing;
using Sitecore.Resources.Media;
using Sitecore.Rules;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using Sitecore.SecurityModel;
using Sitecore.Sites;
using Sitecore.Syndication;
using Sitecore.Visualization;
using Sitecore.Web.Authentication;

namespace Sitecore.NSubstituteUtils
{
  public static class StaticManagersUtils
  {
    public static void ResetStaticManager(Type managerType)
    {
      managerType.TypeInitializer.Invoke(null, null);
    }

    public static void ResetStaticManagers()
    {
      ResetStaticManager(typeof(ItemManager));
      ResetStaticManager(typeof(TemplateManager));
      ResetStaticManager(typeof(PublishManager));
      ResetStaticManager(typeof(HistoryManager));
      ResetStaticManager(typeof(IndexingManager));
      ResetStaticManager(typeof(LanguageManager));
      ResetStaticManager(typeof(ThemeManager));
      ResetStaticManager(typeof(CacheManager));
      ResetStaticManager(typeof(FieldTypeManager));
      ResetStaticManager(typeof(LanguageFallbackFieldValuesManager));
      ResetStaticManager(typeof(ProxyManager));
      ResetStaticManager(typeof(Manager));
      ResetStaticManager(typeof(ValidatorManager));
      ResetStaticManager(typeof(StandardValuesManager));
      ResetStaticManager(typeof(JobManager));
      ResetStaticManager(typeof(ControlManager));
      ResetStaticManager(typeof(PresentationManager));
      ResetStaticManager(typeof(PreviewManager));
      ResetStaticManager(typeof(MediaManager));
      ResetStaticManager(typeof(MediaPathManager));
      ResetStaticManager(typeof(AccessRightManager));
      ResetStaticManager(typeof(AuthorizationManager));
      ResetStaticManager(typeof(RolesInRolesManager));
      ResetStaticManager(typeof(UserManager));
      ResetStaticManager(typeof(DomainManager));
      ResetStaticManager(typeof(FeedManager));
      ResetStaticManager(typeof(LayoutManager));
      ResetStaticManager(typeof(RuleManager));
      ResetStaticManager(typeof(TicketManager));
      ResetStaticManager(typeof(DistributedPublishingManager));
      ResetStaticManager(typeof(LanguageFallbackManager));
      ResetStaticManager(typeof(EventManager));
      ResetStaticManager(typeof(LinkManager));
      ResetStaticManager(typeof(SiteManager));
      ResetStaticManager(typeof(ScriptFactory));
      ResetStaticManager(typeof(CorePipeline));
      ResetStaticManager(typeof(PipelineFactory));
      ResetStaticManager(typeof(Factory));
      ResetStaticManager(typeof(Translate));
      ResetStaticManager(typeof(RuleFactory));
      ResetStaticManager(typeof(ComparerFactory));
      ResetStaticManager(typeof(LinkStrategyFactory));
      ResetStaticManager(typeof(SiteContextFactory));
      ResetStaticManager(typeof(Sitecore.Client));
      ResetStaticManager(typeof(ItemScripts));
      ResetStaticManager(typeof(PlaceholderCacheManager));
      ResetStaticManager(typeof(ArchiveManager));
      ResetStaticManager(typeof(Log));
      ResetStaticManager(typeof(AuthenticationManager));
    }
  }
}
