# LaserCatEyes DotNet SDK - currently in BETA program
![.NET](https://github.com/betalgo/LCE-DotNet-SDK/workflows/.NET/badge.svg?branch=master)

[![LaserCatEyes.HttpClientListener](https://img.shields.io/nuget/v/LaserCatEyes.HttpClientListener?label=nuget.LaserCatEyes.HttpClientListener)](https://www.nuget.org/packages/LaserCatEyes.HttpClientListener/)

[Laser Cat Eyes] is a network monitoring tool that helps mobile app developers diagnose issues between their apps and backend services.

*you can use dotnetstandart SDK's if your app is running under .netcore 3.1*

### Installation & Implementation of HttpClient Listener
1. LaserCatEyes is available through [Nuget](https://www.nuget.org/packages/LaserCatEyes.HttpClientListener/). 
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [LaserCatEyes.HttpClientListener](https://www.nuget.org/packages/LaserCatEyes.HttpClientListener/) from the package manager console:
```
PM> Install-Package LaserCatEyes.HttpClientListener
```

2. In ``Startup`` class ``ConfigureServices`` method inject add Endpoint Listener

#### To listen all HttpClients
```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        if (CurrentEnvironment.IsDevelopment()) //This is a debugging tool, you don't want to run it in production, right!?
        {
            //Seriously don't run it in production environment 
            services.AddLaserCatEyesHttpClientListener(MY_APP_KEY_FROM_LASER_CAT_EYES_PORTAL);
            services.AddLaserCatEyesHttpClientListener(option =>
            {
                option.AppKey = "";
                option.AspCoreEnvironment = "";
                option.Version = "1.2.3.4";
                option.BuildNumber = "1";
            });
        }
    }
```
or
#### Listen only selected HttpClients
```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        if (CurrentEnvironment.IsDevelopment()) //This is a debugging tool, you don't want to run it in production, right!?
        {
            //Seriously don't run it in production environment 
            services.AddLaserCatEyesHttpClientListener(MY_APP_KEY_FROM_LASER_CAT_EYES_PORTAL, listenAllHttpClients: false);
            services.AddHttpClient("myClient", c =>
            {
                //your settings
            }).AddHttpMessageHandler<LaserCatEyesHttpMessageHandler>();
        }
    }
```

[Laser-Cat-Eyes web portal]: <https://portal.lasercateyes.com>
[Laser Cat Eyes]: <https://lasercateyes.com>

## Author

<img src="http://www.betalgo.com/img/logo-dark.png" width="10px"> Betalgo, mail@betalgo.com
