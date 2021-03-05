namespace Microsoft.AspNet.WebFormsDependencyInjection.CastleWindsor.Test
{
    using Castle.MicroKernel;
    using Castle.Windsor;
    using Microsoft.AspNet.WebFormsDependencyInjection.CastleWindsor;
    using Moq;
    using System;
    using System.Web;
    using Xunit;

    public class ContainerServiceProviderTest
    {
        public ContainerServiceProviderTest()
        {
            HttpRuntime.WebObjectActivator = null;
        }

        [Fact]
        public void ContainerServiceProvider_Should_Preserve_Existing_ServiceProvider_And_Initialize_CastleWindsorContainer()
        {
            var existingSP = new Mock<IServiceProvider>();
            var containerSP = new ContainerServiceProvider(existingSP.Object);

            Assert.Same(existingSP.Object, containerSP.NextServiceProvider);
            Assert.NotNull(containerSP.Container);
        }

        [Fact]
        public void GetService_Should_Resolve_Type_EvenIf_CastleWindsor_Cannot_Resolve()
        {
            var containerSP = new ContainerServiceProvider(null);
            var resolvedObj = containerSP.GetService(typeof(TypeToResolveBase));

            Assert.NotNull(resolvedObj);
            Assert.IsType<TypeToResolveBase>(resolvedObj);
        }

        [Fact]
        public void GetService_Should_Use_Saved_ServiceProvider_If_CastleWindsorContainer_Cannot_Resolve()
        {
            var existingSP = new Mock<IServiceProvider>();
            existingSP.Setup(sp => sp.GetService(typeof(TypeToResolveBase))).Returns(new TypeToResolve());
            var containerSP = new ContainerServiceProvider(existingSP.Object);
            var resolvedObj = containerSP.GetService(typeof(TypeToResolveBase));

            Assert.NotNull(resolvedObj);
            Assert.IsType<TypeToResolve>(resolvedObj);
        }

        [Fact]
        public void GetService_Should_Not_Try_CastleWindsorContainer_Again_If_CastleWindsorContainer_Failed_To_Resolve_A_Type()
        {
            var container = new Mock<IWindsorContainer>();
            var containerKernel = new Mock<IKernel>();
            var isFirstCall = true;
            var secondCalled = false;
            var typeToResolve = typeof(TypeToResolveBase);

            containerKernel.Setup(sp => sp.HasComponent(typeToResolve)).Returns(true);
            container.SetupGet(c => c.Kernel).Returns(containerKernel.Object);
            container.Setup(sp => sp.Resolve("", typeToResolve)).Callback(() =>
            {
                if(isFirstCall)
                {
                    isFirstCall = false;
                    throw new ComponentNotFoundException(typeToResolve);
                }
                else
                {
                    secondCalled = true;
                }
            });
            var containerSP = new ContainerServiceProvider(null);
            containerSP.Container = container.Object;
            var resolvedObj = containerSP.GetService(typeToResolve);
            Assert.NotNull(resolvedObj);
            Assert.IsType(typeToResolve, resolvedObj);

            resolvedObj = containerSP.GetService(typeToResolve);
            Assert.NotNull(resolvedObj);
            Assert.IsType(typeToResolve, resolvedObj);
            Assert.False(secondCalled);
        }

        [Fact]
        public void GetService_Should_Cache_Type_That_Cannot_Be_Resolved_By_CastleWindsorContainer()
        {
            var containerSP = new ContainerServiceProvider(null);
            var resolvedObj = containerSP.GetService(typeof(TypeToResolveBase));

            Assert.NotNull(resolvedObj);
            Assert.IsType<TypeToResolveBase>(resolvedObj);
            Assert.True(containerSP.TypeCannotResolveDictionary.ContainsKey(typeof(TypeToResolveBase)));
        }
    }

    class TypeToResolveBase
    {
        protected TypeToResolveBase() { }
    }

    class TypeToResolve : TypeToResolveBase
    {
        public TypeToResolve() { }
    }
}
