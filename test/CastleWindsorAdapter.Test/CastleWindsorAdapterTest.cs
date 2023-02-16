namespace Microsoft.AspNet.WebFormsDependencyInjection.CastleWindsor.Test
{
    using Moq;
    using System;
    using System.Web;
    using Xunit;

    public class CastleWindsorAdapterTest
    {
        public CastleWindsorAdapterTest()
        {
            HttpRuntime.WebObjectActivator = null;
        }

        [Fact]
        public void AddCastleWindsor_Should_Register_WebObjectActivator_And_Return_CastleWindsorContainer()
        {
            var CastleWindsorContainer = CastleWindsorAdapter.AddCastleWindsor();

            Assert.NotNull(HttpRuntime.WebObjectActivator);
            Assert.IsType<ContainerServiceProvider>(HttpRuntime.WebObjectActivator);
            Assert.NotNull(CastleWindsorContainer);
        }

        [Fact]
        public void AddCastleWindsor_Should_Chain_Existing_WebObjectActivator()
        {
            var existingSP = new Mock<IServiceProvider>();
            HttpRuntime.WebObjectActivator = existingSP.Object;

            var CastleWindsorContainer = CastleWindsorAdapter.AddCastleWindsor();

            Assert.NotNull(HttpRuntime.WebObjectActivator);
            Assert.IsType<ContainerServiceProvider>(HttpRuntime.WebObjectActivator);
            Assert.Same(existingSP.Object, ((ContainerServiceProvider)HttpRuntime.WebObjectActivator).NextServiceProvider);
            Assert.NotNull(CastleWindsorContainer);
        }
    }
}
