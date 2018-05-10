

namespace Microsoft.AspNet.WebFormsDependencyInjection.Unity
{
    using global::Unity;
    using global::Unity.Exceptions;
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using System.Web.Hosting;

    /// <summary>
    /// The Unity adapter for WebObjectActivator
    /// </summary>
    class ContainerServiceProvider : IServiceProvider, IRegisteredObject
    {
        private const int TypesCannontResolveCacheCap = 100000;
        private readonly IServiceProvider _next;
        private readonly ConcurrentDictionary<Type, bool> _typesCannotResolve = new ConcurrentDictionary<Type, bool>();

        public ContainerServiceProvider(IServiceProvider next)
        {
            _next = next;
            HostingEnvironment.RegisterObject(this);
        }

        /// <summary>
        /// Implementation of IServiceProvider. Asp.net will call this method to
        /// create the instances of Page/UserControl/HttpModule etc.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            //
            // Try unresolvable types
            if (_typesCannotResolve.ContainsKey(serviceType))
            { 
                return DefaultCreateInstance(serviceType);
            }

            //
            // Try the container
            object result = null;

            try
            {
                result = Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                // Ignore and continue
            }

            //
            // Try the next provider
            if (result == null)
            {
                result = _next?.GetService(serviceType);
            }

            //
            // Default activation
            if (result == null)
            {
                if ((result = DefaultCreateInstance(serviceType)) != null)
                { 
                    // Cache it
                    if (_typesCannotResolve.Count < TypesCannontResolveCacheCap)
                    { 
                        _typesCannotResolve.TryAdd(serviceType, true);
                    }
                }
            }

            return result;
        }

        public IUnityContainer Container { get; } = new UnityContainer();
        
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
    }
}
