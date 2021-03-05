namespace Microsoft.AspNet.WebFormsDependencyInjection.CastleWindsor.Test
{
    using Moq;
    using System;
    using System.Web;
    using Xunit;

    public class HttpApplicationExtensionsTest
    {
        public HttpApplicationExtensionsTest()
        {
            HttpRuntime.WebObjectActivator = null;
        }

        [Fact]
        public void AddCastleWindsor_Should_Throw_ArgumentNullException_If_HttpApplication_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                ((HttpApplication)null).AddCastleWindsor();
            });
        }

        [Fact]
        public void AddCastleWindsor_Should_Register_WebObjectActivator_With_ContainerServiceProvider()
        {
            var app = new HttpApplication();
            var container = app.AddCastleWindsor();

            Assert.NotNull(container);
            Assert.NotNull(HttpRuntime.WebObjectActivator);
            Assert.IsType<ContainerServiceProvider>(HttpRuntime.WebObjectActivator);
        }

        [Fact]
        public void GetCastleWindsorContainer_Should_Return_CastleWindsorContainer()
        {
            var app = new HttpApplication();
            app.AddCastleWindsor();
            var container = app.GetCastleWindsorContainer();

            Assert.NotNull(container);
        }

        [Fact]
        public void GetCastleWindsorContainer_Should_Return_Null_If_Registered_WebObjectActivator_Is_Not_ContainerServiceProvider()
        {
            var app = new HttpApplication();
            var existingSP = new Mock<IServiceProvider>();
            HttpRuntime.WebObjectActivator = existingSP.Object;

            var container = app.GetCastleWindsorContainer();
            Assert.Null(container);
        }
    }
}
