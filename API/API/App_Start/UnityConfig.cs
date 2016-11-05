using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using System;
using System.Web.Http;

namespace API
{
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() => { var container = new UnityContainer(); Data.Npgsql.Configuration.IoCConfig.ConfigureIoC(container); return container; });
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        public static void RegisterComponents(HttpConfiguration config)
        {
            config.DependencyResolver = new UnityHierarchicalDependencyResolver(GetConfiguredContainer());
        }
    }
}