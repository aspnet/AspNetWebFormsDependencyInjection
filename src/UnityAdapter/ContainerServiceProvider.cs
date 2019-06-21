// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See the License.txt file in the project root for full license information.

namespace Microsoft.AspNet.WebFormsDependencyInjection.Unity
{
    using global::Unity;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Hosting;

    /// <summary>
    /// The Unity adapter for WebObjectActivator
    /// </summary>
    internal class ContainerServiceProvider : IServiceProvider, IRegisteredObject, IDisposable
    {
        public IUnityContainer Container { get; internal set; } = new UnityContainer();

        private const int TypesCannontResolveCacheCap = 100000;

        private readonly ConcurrentDictionary<Type, bool> _typesCannotResolve = new ConcurrentDictionary<Type, bool>();

        internal IDictionary<Type, bool> TypeCannotResolveDictionary
        {
            get { return _typesCannotResolve; }
        }

        internal IServiceProvider NextServiceProvider { get; }

        public ContainerServiceProvider(IServiceProvider next)
        {
            NextServiceProvider = next;
            HostingEnvironment.RegisterObject(this);
        }

        /// <summary>
        /// Implementation of IServiceProvider. Asp.net will call this method to
        /// create the instances of Page/UserControl/HttpModule etc.
        /// </summary>
        /// <param name="serviceType"></param>
        public object GetService(Type serviceType)
        {
            // Try unresolvable types
            if (_typesCannotResolve.ContainsKey(serviceType)) { return DefaultCreateInstance(serviceType); }

            // Try the container
            object result = null;

            try
            { result = Container.Resolve(serviceType); }
            catch (ResolutionFailedException) { } // Ignore and continue

            // Try the next provider
            if (result == null)
            {
                result = NextServiceProvider?.GetService(serviceType);
            }

            // Default activation
            if (result == null && (result = DefaultCreateInstance(serviceType)) != null)
            {
                // Cache it
                if (_typesCannotResolve.Count < TypesCannontResolveCacheCap)
                {
                    _typesCannotResolve.TryAdd(serviceType, true);
                }
            }

            return result;
        }

        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);

            Container.Dispose();
        }

        protected virtual object DefaultCreateInstance(Type type)
        {
            return Activator.CreateInstance(
                type,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance,
                null,
                null,
                null);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
