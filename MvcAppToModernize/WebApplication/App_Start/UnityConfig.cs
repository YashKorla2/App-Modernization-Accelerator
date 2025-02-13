using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc; 
using Services;

namespace WebApplication
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var unityContainer = new UnityContainer();
            RegisterDependencies(unityContainer);
            return unityContainer;
        });
        public static IUnityContainer Container => container.Value;

        private static void RegisterDependencies(IUnityContainer unityContainer)
        {
            // Register your services and repositories here
            unityContainer.RegisterType<IHomeService, HomeService>();
        }

        public static void RegisterUnity()
        {
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }
    }
}
