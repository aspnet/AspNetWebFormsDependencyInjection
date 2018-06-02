// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See the License.txt file in the project root for full license information.

namespace Microsoft.AspNet.WebFormsDependencyInjection.Unity.Test
{
    using Moq;
    using Microsoft.AspNet.WebFormsDependencyInjection.Unity;
    using System;
    using Xunit;
    using global::Unity.Exceptions;
    using global::Unity;
    using System.Web;

    public class ContainerServiceProviderTest
    {
        public ContainerServiceProviderTest()
        {
            HttpRuntime.WebObjectActivator = null;
        }

        [Fact]
        public void ContainerServiceProvider_Should_Preserve_Existing_ServiceProvider_And_Initialize_UnityContainer()
        {
            var existingSP = new Mock<IServiceProvider>();
            var containerSP = new ContainerServiceProvider(existingSP.Object);

            Assert.Same(existingSP.Object, containerSP.NextServiceProvider);
            Assert.NotNull(containerSP.Container);
        }

        [Fact]
        public void GetService_Should_Resolve_Type_EvenIf_Unity_Cannot_Resolve()
        {
            var containerSP = new ContainerServiceProvider(null);
            var resolvedObj = containerSP.GetService(typeof(TypeToResolveBase));

            Assert.NotNull(resolvedObj);
            Assert.IsType<TypeToResolveBase>(resolvedObj);
        }

        [Fact]
        public void GetService_Should_Use_Saved_ServiceProvider_If_UnityContainer_Cannot_Resolve()
        {
            var existingSP = new Mock<IServiceProvider>();
            existingSP.Setup(sp => sp.GetService(typeof(TypeToResolveBase))).Returns(new TypeToResolve());
            var containerSP = new ContainerServiceProvider(existingSP.Object);
            var resolvedObj = containerSP.GetService(typeof(TypeToResolveBase));

            Assert.NotNull(resolvedObj);
            Assert.IsType<TypeToResolve>(resolvedObj);
        }

        [Fact]
        public void GetService_Should_Not_Try_UnityContainer_Again_If_UnityContainer_Failed_To_Resolve_A_Type()
        {
            var container = new Mock<IUnityContainer>();
            var isFirstCall = true;
            var secondCalled = false;
            var typeToResolve = typeof(TypeToResolveBase);

            container.Setup(sp => sp.Resolve(typeToResolve, "", null)).Callback(() =>
            {
                if(isFirstCall)
                {
                    isFirstCall = false;
                    throw new ResolutionFailedException(typeToResolve, "", "", null);
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
        public void GetService_Should_Cache_Type_That_Cannot_Be_Resolved_By_UnityContainer()
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
