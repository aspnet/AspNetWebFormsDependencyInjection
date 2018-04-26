

namespace Microsoft.AspNet.WebFormsDependencyInjection.Unity
{
    using global::Unity;
    using global::Unity.Exceptions;
    using Microsoft.AspNet.WebFormsDependencyInjection.Unity.Resources;
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using System.Web;

    /// <summary>
    /// The Unity adapter for WebObjectActivator
    /// </summary>
    public class UnityContainerAdapter : IServiceProvider
    {
        private static UnityContainerAdapter _adapter;
        private static object _lock = new object();

        private IUnityContainer _container;
        private ConcurrentDictionary<Type, bool> _typesCannotResolve = new ConcurrentDictionary<Type, bool>();

        internal Func<Type, object> CreateNonPublicInstance =
                    (serviceType) => Activator.CreateInstance(
                        serviceType,
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance,
                        null,
                        null,
                        null);

        internal UnityContainerAdapter(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Implementation of IServiceProvider. Asp.net will call this method to
        /// create the instances of Page/UserControl/HttpModule etc.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            if (_typesCannotResolve.ContainsKey(serviceType))
            {
                return CreateNonPublicInstance(serviceType);
            }

            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                _typesCannotResolve.TryAdd(serviceType, true);
                return CreateNonPublicInstance(serviceType);
            }
        }

        /// <summary>
        /// Plugin the adapter to WebObjectActivator and register the types in Unity container
        /// </summary>
        /// <param name="registerTypes"></param>
        /// <returns></returns>
        public static IUnityContainer RegisterWebObjectActivator(Action<IUnityContainer> registerTypes)
        {
            if (registerTypes == null)
            {
                throw new ArgumentNullException(nameof(registerTypes));
            }
            if(_adapter != null)
            {
                throw new InvalidOperationException(SR.Cannot_Register_WebObjectActivator_More_Than_Once);
            }

            lock (_lock)
            {
                if (_adapter == null)
                {
                    var container = new UnityContainer();
                    _adapter = new UnityContainerAdapter(container);
                    HttpRuntime.WebObjectActivator = _adapter;
                    registerTypes(container);

                    return container;
                }
                else
                {
                    throw new InvalidOperationException(SR.Cannot_Register_WebObjectActivator_More_Than_Once);
                }
            }
        }
    }
}
