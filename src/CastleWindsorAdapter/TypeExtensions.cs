namespace Microsoft.AspNet.WebFormsDependencyInjection.CastleWindsor
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;

    public static class TypeExtensions
    {
        public static bool IsWebFormsComponent(this Type type)
        {
            return (typeof(UserControl).IsAssignableFrom(type) ||
                    typeof(IHttpHandler).IsAssignableFrom(type));
        }
    }
}
