using System;
using System.Threading;
using Sitecore.DependencyInjection;

namespace Sitecore.NSubstituteUtils
{
  public class FakeServiceProviderWrapper : IDisposable
  {
    private static readonly object syncObj = new Object();

    public FakeServiceProviderWrapper(IServiceProvider provider)
    {
      Monitor.Enter(syncObj);
      StaticManagersUtils.ResetStaticManagers();
      ServiceLocator.SetServiceProvider(provider);
    }

    public void Dispose()
    {
      ServiceLocator.Reset();
      StaticManagersUtils.ResetStaticManagers();
      Monitor.Exit(syncObj);
    }
  }
}
