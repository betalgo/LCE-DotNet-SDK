
[![Welcome](https://repository-images.githubusercontent.com/305744454/59fff700-30f5-11eb-8a67-d706102bf31c)](https://repository-images.githubusercontent.com/305744454/59fff700-30f5-11eb-8a67-d706102bf31c)
# LaserCatEyes DotNet SDK - currently in BETA program
![.NET](https://github.com/betalgo/LCE-DotNet-SDK/workflows/.NET/badge.svg?branch=master)

[![LaserCatEyes.EndpointListener](https://img.shields.io/nuget/v/LaserCatEyes.EndpointListener?label=nuget.LaserCatEyes.EndpointListener)](https://www.nuget.org/packages/LaserCatEyes.EndpointListener/)

[![LaserCatEyes.HttpClientListener](https://img.shields.io/nuget/v/LaserCatEyes.HttpClientListener?label=nuget.LaserCatEyes.HttpClientListener)](https://www.nuget.org/packages/LaserCatEyes.HttpClientListener/)

[![LaserCatEyes.HttpClientListener.DotNetStandard](https://img.shields.io/nuget/v/LaserCatEyes.HttpClientListener.DotNetStandard?label=nuget.LaserCatEyes.HttpClientListener.DotNetStandard)](https://www.nuget.org/packages/LaserCatEyes.HttpClientListener.DotNetStandard/)

[![LaserCatEyes.DataServiceSdk](https://img.shields.io/nuget/v/LaserCatEyes.DataServiceSdk?label=nuget.LaserCatEyes.DataServiceSdk)](https://www.nuget.org/packages/LaserCatEyes.DataServiceSdk/)

[![LaserCatEyes.DataServiceSdk.DotNetStandard](https://img.shields.io/nuget/v/LaserCatEyes.DataServiceSdk.DotNetStandard?label=nuget.LaserCatEyes.DataServiceSdk.DotNetStandard)](https://www.nuget.org/packages/LaserCatEyes.DataServiceSdk.DotNetStandard/)

[![LaserCatEyes.WCFListener](https://img.shields.io/nuget/v/LaserCatEyes.DataServiceSdk.DotNetStandard?label=nuget.LaserCatEyes.WCFListener)](https://www.nuget.org/packages/LaserCatEyes.WCFListener/)


Laser Cat Eyes is a network monitoring tool that helps mobile app developers diagnose issues between their apps and backend services.

### How do I get started?
You need to get **APP_KEY** from [Laser-Cat-Eyes web portal]
There are different ways to integrate Laser Cat Eyes to your project, feel free to pick one of them or you can enjoy all of them same time. 
1. You can install [iOS](https://github.com/betalgo/LCE-iOS-SDK) or Android(not avaliable yet) libraries to your application which will provide more insgiht about device. 
2. You can install [![](https://img.shields.io/nuget/v/LaserCatEyes.EndpointListener?label=nuget.LaserCatEyes.EndpointListener)](https://www.nuget.org/packages/LaserCatEyes.EndpointListener/) which will show you all **incoming requests** to your .Net server.
3. You can install [![](https://img.shields.io/nuget/v/LaserCatEyes.HttpClientListener?label=nuget.LaserCatEyes.HttpClientListener)](https://www.nuget.org/packages/LaserCatEyes.HttpClientListener/) which will show you all **outgoing request** from your .Net server.
4. You can install [![](https://img.shields.io/nuget/v/LaserCatEyes.WCFListener?label=nuget.LaserCatEyes.WCFListener)](https://www.nuget.org/packages/LaserCatEyes.WCFListener/) which will show you all **outgoing WCF/SOAP** network calls from your .Net server.
5. You can develop your custom listener using [![](https://img.shields.io/nuget/v/LaserCatEyes.DataServiceSdk?label=nuget.LaserCatEyes.DataServiceSdk)](https://www.nuget.org/packages/LaserCatEyes.DataServiceSdk/)

*you can use dotnetstandart SDK's if your app is running under .netcore 3.1*
### Hot to get your APP_KEY :
1. Create an account from [Laser-Cat-Eyes web portal]
2. Create an app
3. After the hitting save button you should be able to see your **APP KEY**

### Installation & Implementation of EnpointListener
1. LaserCatEyes is available through [Nuget](https://www.nuget.org/packages/LaserCatEyes.EndpointListener/). 
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [LaserCatEyes.EndpointListener](https://www.nuget.org/packages/LaserCatEyes.EndpointListener/) from the package manager console:
```
PM> Install-Package LaserCatEyes.EndpointListener
```

2. In ``Startup`` class ``Configure`` method inject middleware
```csharp
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        ...
        if (env.IsDevelopment())//This is a debugging tool, you don't want to run it in production, right!?
        {
           ... 
           //Seriously don't run it in production environment 
           app.UseLaserCatEyesEndpointListenerMiddleware();           
        }
       ...
    }
```
3. In ``Startup`` class ``ConfigureServices`` method inject add Endpoint Listener

```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        if (env.IsDevelopment())//This is a debugging tool, you don't want to run it in production, right!?
        {
           ... 
           //Seriously don't run it in production environment 
           services.AddLaserCatEyesEndpointListener(MY_APP_KEY_FROM_LASER_CAT_EYES_PORTAL);
           //OR (more option will be available soon)
           services.AddLaserCatEyesHttpListener(option =>
           {
               option.AppKey = MY_APP_KEY_FROM_LASER_CAT_EYES_PORTAL;
               option.AspCoreEnvironment = "STAGE";
               option.Version = "1.2.3.4";
               option.BuildNumber = "1";
           });               
        }
       ...
    }
```
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
            }).AddLaserCatEyesHttpClientListener();
        }
    }
```


[Laser-Cat-Eyes web portal]: <https://portal.lasercateyes.com>


## Author

<img src="http://www.betalgo.com/img/logo-dark.png" width="10px"> Betalgo, mail@betalgo.com
