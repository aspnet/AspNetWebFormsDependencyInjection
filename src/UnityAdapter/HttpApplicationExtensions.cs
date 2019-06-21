// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See the License.txt file in the project root for full license information.

namespace Microsoft.AspNet.WebFormsDependencyInjection.Unity
{
    using global::Unity;
    using System;
    using System.Web;

    /// <summary>
    /// Extension methods of HttpApplication that help use Unity container
    /// </summary>
    public static class HttpApplicationExtensions
    {
        /// <summary>
        /// Extension method to AddUnity.
        /// </summary>
        /// <param name="application"></param>
        public static IUnityContainer AddUnity(this HttpApplication application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            return UnityAdapter.AddUnity();
        }

        /// <summary>
        /// Extension method to acquire the internal UnityContainer
        /// </summary>
        /// <param name="application"></param>
        public static IUnityContainer GetUnityContainer(this HttpApplication application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            return UnityAdapter.GetContainer();
        }
    }
}
