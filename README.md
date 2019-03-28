## Introduction
[Dependency injection](https://en.wikipedia.org/wiki/Dependency_injection) design pattern is widely used in modern applications. It decouples objects to the extent that no client code has to be changed simply because an object it depends on needs to be changed to a different one. It brings you a lot of [benefits](http://tutorials.jenkov.com/dependency-injection/dependency-injection-benefits.html), like reduced dependency, more reusable code, more testable code, etc. However, it was very difficult to use dependency injection in WebForms application before. This is not an issue in .Net Framework 4.7.2 anymore. Dependency injection is natively supported in WebForms applications. 

This project demonstrates building a dependency injection adapter for the [Unity](https://github.com/unitycontainer/unity) IoC container as a point of reference for other adapter authors. Please note that this implementation is for demonstration purposes only and is not being actively maintained.

## How to use
1. Make sure your web project is targeting .NET Framework 4.7.2. You can download .NET Framework 4.7.2 developer pack from [here](https://www.microsoft.com/net/download/thank-you/net472-developer-pack). Check web.config and targetFramework in httpRuntime section should be 4.7.2.
```
  <system.web>
    <compilation debug="true" targetFramework="4.7.2"/>
    <httpRuntime targetFramework="4.7.2"/>
  </system.web>
```
2. Install Microsoft.AspNet.WebFormsDependencyInjection.Unity nupkg in your project.
3. Open Global.asax and register the types in UnityContainer.
```
        protected void Application_Start(object sender, EventArgs e)
        {
            var container = this.AddUnity();

            container.RegisterType<ISomeInterface, SomeImplementation>();
        }
```

## How to build
1. Open a [VS developer command prompt](https://docs.microsoft.com/en-us/dotnet/framework/tools/developer-command-prompt-for-vs)
2. Run build.cmd. This will build Nuget package and run all the unit tests.
3. All the build artifacts will be under AspNetWebFormsDependencyInjection\bin\Release\ folder.

## How to contribute
Information on contributing to this repo is in the [Contributing Guide](CONTRIBUTING.md).
