using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc; 
using Services;
using Repositories;

namespace WebApplication
{
    /// <summary>
    /// Configuration class for Unity dependency injection container.
    /// Handles registration and resolution of dependencies throughout the application.
    /// </summary>
    public static class UnityConfig
    {
        // Lazy initialization of Unity container ensures it's only created when first accessed
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var unityContainer = new UnityContainer();
            RegisterDependencies(unityContainer);
            return unityContainer;
        });

        // Public property to access the Unity container instance
        public static IUnityContainer Container => container.Value;

        /// <summary>
        /// Registers all application dependencies in the Unity container.
        /// Maps interfaces to their concrete implementations.
        /// The path for all C# classes of all the registered dependencies are as follows:
        /// 1. Repository Dependencies: MvcAppToModernize\Repositories
        /// 2. Service Dependencies: MvcAppToModernize\Services
        /// </summary>
        private static void RegisterDependencies(IUnityContainer unityContainer)
        {
            // Register your services and repositories here
            // Product-related dependencies
            unityContainer.RegisterType<IProductRepository, ProductRepository>();
            unityContainer.RegisterType<IProductService, ProductService>();
            
            // Cart-related dependencies
            unityContainer.RegisterType<ICartRepository, CartRepository>();
            unityContainer.RegisterType<ICartService, CartService>();
        }

        /// <summary>
        /// Configures MVC to use Unity as its dependency resolver.
        /// Should be called during application startup.
        /// </summary>
        public static void RegisterUnity()
        {
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }
    }
}
