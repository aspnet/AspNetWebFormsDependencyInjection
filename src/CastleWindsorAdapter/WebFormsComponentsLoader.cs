namespace Microsoft.AspNet.WebFormsDependencyInjection.CastleWindsor
{
    using global::Castle.MicroKernel.Registration;
    using global::Castle.MicroKernel.Resolvers;
    using System;
    using System.Collections;

    /// <summary>
    /// Lazy loader for dynamic ASP.NET types created in runtime
    /// </summary>
    public class WebFormsComponentsLoader : ILazyComponentLoader
    {
        public IRegistration Load(string name, Type service, IDictionary arguments)
        {
            if (service == null)
            {
                return null;
            }

            if (service.IsWebFormsComponent())
            {
                return Component.For(service)
                                .LifeStyle.Transient
                                .NamedAutomatically("webforms");
            }

            return null;
        }
    }
}
