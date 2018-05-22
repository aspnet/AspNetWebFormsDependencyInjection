// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See the License.txt file in the project root for full license information.

namespace Microsoft.AspNet.WebFormsDependencyInjection.Unity
{
    using System;
    using System.Web;
    using global::Unity;

    /// <summary>
    /// Extension methods of HttpApplication that help use Unity container
    /// </summary>
    public static class HttpApplicationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static IUnityContainer AddUnity(this HttpApplication application)
        { 
            if (application == null)
            { 
                throw new ArgumentNullException(nameof(application));
            }

            return UnityAdapter.AddUnity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer GetUnityContainer(this HttpApplication application)
        {
            return UnityAdapter.GetContainer();
        }
    }
}
