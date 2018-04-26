[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.UnityActivator), nameof($rootnamespace$.UnityActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof($rootnamespace$.UnityActivator), nameof($rootnamespace$.UnityActivator.Shutdown))]
namespace $rootnamespace$
{
    using Unity;
    using Microsoft.AspNet.WebFormsDependencyInjection.Unity;

    /// <summary>
    /// Provides the bootstrapping for integrating Unity with ASP.NET WebForms.
    /// </summary>
    public static class UnityActivator
    {
        private static IUnityContainer container;

        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Start() 
        {
            container = UnityContainerAdapter.RegisterWebObjectActivator(RegisterTypes);
        }

        /// <summary>
        /// Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown()
        {
            container.Dispose();
        }

        /// <summary>
        /// Register the types in UnityContainer
        /// </summary>
        public static void RegisterTypes(IUnityContainer container) {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<ISomeInterface, SomeImplementation>();
        }
    }
}