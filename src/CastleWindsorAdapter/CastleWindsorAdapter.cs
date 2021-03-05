namespace Microsoft.AspNet.WebFormsDependencyInjection.CastleWindsor
{
    using Castle.Windsor;
    using System.Web;

    /// <summary>
    /// Extension methods of HttpApplication that help use Castle Windsor container
    /// </summary>
    public static class CastleWindsorAdapter
    {
        private static object _lock = new object();

        /// <summary>
        /// Add a new Castle Windsor container in asp.net application. If there is WebObjectActivator already registered,
        /// it will be chained up. When the new WebObjectActivator can't resolve the type, the previous WebObjectActivator
        /// will be used. If the previous WebObjectActivator can't resolve it either, DefaultCreateInstance will be used
        /// which creates instance through none public default constructor based on reflection.
        /// </summary>
        /// <returns></returns>
        public static IWindsorContainer AddCastleWindsor()
        {
            lock (_lock)
            {
                HttpRuntime.WebObjectActivator = new ContainerServiceProvider(HttpRuntime.WebObjectActivator);

                return GetContainer();
            }
        }

        /// <summary>
        /// Get most recent added Castle Windsor container
        /// </summary>
        /// <returns></returns>
        public static IWindsorContainer GetContainer()
        {
            return (HttpRuntime.WebObjectActivator as ContainerServiceProvider)?.Container;
        }
    }
}