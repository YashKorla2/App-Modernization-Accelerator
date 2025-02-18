using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc; 
using Services;
using Repositories;

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
            unityContainer.RegisterType<IProductRepository, ProductRepository>();
            unityContainer.RegisterType<IProductService, ProductService>();
            
            unityContainer.RegisterType<ICartRepository, CartRepository>();
            unityContainer.RegisterType<ICartService, CartService>();
        }

        public static void RegisterUnity()
        {
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }
    }
}
