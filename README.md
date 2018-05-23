## Introduction


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
