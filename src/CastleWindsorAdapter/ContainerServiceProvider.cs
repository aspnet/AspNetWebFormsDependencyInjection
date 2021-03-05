namespace Microsoft.AspNet.WebFormsDependencyInjection.CastleWindsor
{
    using global::Castle.MicroKernel.Registration;
    using global::Castle.MicroKernel.Resolvers;
    using global::Castle.Windsor;
    using global::Castle.MicroKernel;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Hosting;

    /// <summary>
    /// The Castle Windsor adapter for WebObjectActivator
    /// </summary>
    class ContainerServiceProvider : IServiceProvider, IRegisteredObject
    {
        private const int TypesCannontResolveCacheCap = 100000;
        private readonly IServiceProvider _next;
        private readonly ConcurrentDictionary<Type, bool> _typesCannotResolve = new ConcurrentDictionary<Type, bool>();

        public IWindsorContainer Container { get; internal set; }

        public ContainerServiceProvider(IServiceProvider next)
        {
            _next = next;
            HostingEnvironment.RegisterObject(this);

            Container = new WindsorContainer();
            Container.Register(Component.For<ILazyComponentLoader>().ImplementedBy<WebFormsComponentsLoader>());
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

            //
            // registered component or WebForms type
            if (Container.Kernel.HasComponent(serviceType) || serviceType.IsWebFormsComponent())
            {
                try
                {
                    result = Container.Resolve(serviceType);
                }
                catch (ComponentNotFoundException)
                {
                    // Ignore and continue
                }
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
                    // Cache it so we don't need to bother container again
                    if (_typesCannotResolve.Count < TypesCannontResolveCacheCap)
                    {
                        _typesCannotResolve.TryAdd(serviceType, true);
                    }
                }
            }

            return result;
        }

        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);
            Container.Dispose();
        }

        internal IServiceProvider NextServiceProvider
        {
            get { return _next; }
        }

        internal IDictionary<Type, bool> TypeCannotResolveDictionary
        {
            get { return _typesCannotResolve; }
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