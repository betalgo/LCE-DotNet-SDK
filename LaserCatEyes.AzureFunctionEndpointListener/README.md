# LaserCatEyes DotNet Azure Function SDK - currently in BETA program
![.NET](https://github.com/betalgo/LCE-DotNet-SDK/workflows/.NET/badge.svg?branch=master)

[![LaserCatEyes.AzureFunctionEndpointListener](https://img.shields.io/nuget/v/LaserCatEyes.EndpointListener?label=nuget.LaserCatEyes.EndpointListener)](https://www.nuget.org/packages/LaserCatEyes.EndpointListener/)
[![LaserCatEyes.DataServiceSdk](https://img.shields.io/nuget/v/LaserCatEyes.DataServiceSdk?label=nuget.LaserCatEyes.DataServiceSdk)](https://www.nuget.org/packages/LaserCatEyes.DataServiceSdk/)

[Laser Cat Eyes] is a network monitoring tool that helps mobile app developers diagnose issues between their apps and backend services.

*you can use dotnetstandart SDK's if your app is running under .net 6*

### Installation & Implementation of EnpointListener
1. LaserCatEyes is available through [Nuget](https://www.nuget.org/packages/LaserCatEyes.AzureFunctionEndpointListener/). 
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [LaserCatEyes.AzureFunctionEndpointListener](https://www.nuget.org/packages/LaserCatEyes.AzureFunctionEndpointListener/) from the package manager console:
```
PM> Install-Package LaserCatEyes.AzureFunctionEndpointListener
```

2. In ``Startup`` class ``Configure`` method inject middleware
```csharp
   var host = new HostBuilder()
   .ConfigureFunctionsWorkerDefaults(builder =>
    {
        if (env.IsDevelopment())//This is a debugging tool, you don't want to run it in production, right!?
        {
            builder.Services.AddLaserCatAzureFunctionEyesEndpointListener(MY_APP_KEY_FROM_LASER_CAT_EYES_PORTAL);
            builder.UseLaserCatEyesEndpointListenerMiddleware();
        }
    })    
   
```

[Laser-Cat-Eyes web portal]: <https://portal.lasercateyes.com>
[Laser Cat Eyes]: <https://lasercateyes.com>

## Author

<img src="http://www.betalgo.com/img/logo-dark.png" width="10px"> Betalgo, mail@betalgo.com
