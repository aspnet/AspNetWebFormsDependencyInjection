// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See the License.txt file in the project root for full license information.

namespace Microsoft.AspNet.WebFormsDependencyInjection.Unity
{
    using System.Web;
    using global::Unity;

    /// <summary>
    /// Extension methods of HttpApplication that help use Unity container
    /// </summary>
    public static class UnityAdapter
    {
        private static object _lock = new object();

        /// <summary>
        /// Add a new Unity container in asp.net application. If there is WebObjectActivator already registered,
        /// it will be chained up. When the new WebObjectActivator can't resolve the type, the previous WebObjectActivator
        /// will be used. If the previous WebObjectActivator can't resolve it either, DefaultCreateInstance will be used
        /// which creates instance through none public default constructor based on reflection.
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer AddUnity()
        {
            lock (_lock)
            {
                HttpRuntime.WebObjectActivator = new ContainerServiceProvider(HttpRuntime.WebObjectActivator);
                
                return GetContainer();
            }
        }

        /// <summary>
        /// Get most recent added Unity container
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer GetContainer()
        {
            return (HttpRuntime.WebObjectActivator as ContainerServiceProvider)?.Container;
        }
    }
}
