# LaserCatEyes DotNet SDK - currently in BETA program
![.NET](https://github.com/betalgo/LCE-DotNet-SDK/workflows/.NET/badge.svg?branch=master)

[![LaserCatEyes.HttpClientListener.DotNetStandard](https://img.shields.io/nuget/v/LaserCatEyes.HttpClientListener.DotNetStandard?label=nuget.LaserCatEyes.HttpClientListener.DotNetStandard)](https://www.nuget.org/packages/LaserCatEyes.HttpClientListener.DotNetStandard/)

[Laser Cat Eyes] is a network monitoring tool that helps mobile app developers diagnose issues between their apps and backend services.

### Hot to get your APP_KEY :
1. Create an account from [Laser-Cat-Eyes web portal]
2. Create an app
3. After the hitting save button you should be able to see your **APP KEY**

```
### Installation & Implementation of HttpClient Listener
1. LaserCatEyes is available through [Nuget](https://www.nuget.org/packages/LaserCatEyes.HttpClientListener.DotNetStandard/). 
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [LaserCatEyes.HttpClientListener](https://www.nuget.org/packages/LaserCatEyes.HttpClientListener.DotNetStandard/) from the package manager console:
```
PM> Install-Package LaserCatEyes.HttpClientListener.DotNetStandard
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
