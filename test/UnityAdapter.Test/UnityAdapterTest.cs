// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See the License.txt file in the project root for full license information.

namespace Microsoft.AspNet.WebFormsDependencyInjection.Unity.Test
{
    using Moq;
    using System;
    using System.Web;
    using Xunit;

    public class UnityAdapterTest
    {
        public UnityAdapterTest()
        {
            HttpRuntime.WebObjectActivator = null;
        }

        [Fact]
        public void AddUnity_Should_Register_WebObjectActivator_And_Return_UnityContainer()
        {
            var unityContainer = UnityAdapter.AddUnity();

            Assert.NotNull(HttpRuntime.WebObjectActivator);
            Assert.IsType<ContainerServiceProvider>(HttpRuntime.WebObjectActivator);
            Assert.NotNull(unityContainer);
        }

        [Fact]
        public void AddUnity_Should_Chain_Existing_WebObjectActivator()
        {
            var existingSP = new Mock<IServiceProvider>();
            HttpRuntime.WebObjectActivator = existingSP.Object;

            var unityContainer = UnityAdapter.AddUnity();

            Assert.NotNull(HttpRuntime.WebObjectActivator);
            Assert.IsType<ContainerServiceProvider>(HttpRuntime.WebObjectActivator);
            Assert.Same(existingSP.Object, ((ContainerServiceProvider)HttpRuntime.WebObjectActivator).NextServiceProvider);
            Assert.NotNull(unityContainer);
        }
    }
}
