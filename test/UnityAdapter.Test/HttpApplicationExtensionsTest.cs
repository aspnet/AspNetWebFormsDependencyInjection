// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See the License.txt file in the project root for full license information.

namespace Microsoft.AspNet.WebFormsDependencyInjection.Unity.Test
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
        public void AddUnity_Should_Throw_ArgumentNullException_If_HttpApplication_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                ((HttpApplication)null).AddUnity();
            });
        }

        [Fact]
        public void AddUnity_Should_Register_WebObjectActivator_With_ContainerServiceProvider()
        {
            var app = new HttpApplication();
            var container = app.AddUnity();

            Assert.NotNull(container);
            Assert.NotNull(HttpRuntime.WebObjectActivator);
            Assert.IsType<ContainerServiceProvider>(HttpRuntime.WebObjectActivator);
        }

        [Fact]
        public void GetUnityContainer_Should_Return_UnityContainer()
        {
            var app = new HttpApplication();
            app.AddUnity();
            var container = app.GetUnityContainer();

            Assert.NotNull(container);
        }

        [Fact]
        public void GetUnityContainer_Should_Return_Null_If_Registered_WebObjectActivator_Is_Not_ContainerServiceProvider()
        {
            var app = new HttpApplication();
            var existingSP = new Mock<IServiceProvider>();
            HttpRuntime.WebObjectActivator = existingSP.Object;

            var container = app.GetUnityContainer();
            Assert.Null(container);
        }
    }
}
