namespace Microsoft.AspNet.WebFormsDependencyInjection.CastleWindsor
{
    using global::Castle.Windsor;
    using System;
    using System.Web;

    /// <summary>
    /// Extension methods of HttpApplication that help use Castle Windsor container
    /// </summary>
    public static class HttpApplicationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static IWindsorContainer AddCastleWindsor(this HttpApplication application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return CastleWindsorAdapter.AddCastleWindsor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IWindsorContainer GetCastleWindsorContainer(this HttpApplication application)
        {
            return CastleWindsorAdapter.GetContainer();
        }
    }
}