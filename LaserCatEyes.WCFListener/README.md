# LaserCatEyes WCF Listener SDK - currently in BETA program
![.NET](https://github.com/betalgo/LCE-DotNet-SDK/workflows/.NET/badge.svg?branch=master)

[![LaserCatEyes.WCFListener](https://img.shields.io/nuget/v/LaserCatEyes.HttpClientListener.DotNetStandard?label=nuget.LaserCatEyes.WCFListener)](https://www.nuget.org/packages/LaserCatEyes.WCFListener/)

[Laser Cat Eyes] is a network monitoring tool that helps mobile app developers diagnose issues between their apps and other services.

### Hot to get your APP_KEY :
1. Create an account from [Laser-Cat-Eyes web portal]
2. Create an app
3. After the hitting save button you should be able to see your **APP KEY**

```
### Installation & Implementation of WCF Listener (Inspector)
1. LaserCatEyes is available through [Nuget](https://www.nuget.org/packages/LaserCatEyes.WCFListener/). 
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [LaserCatEyes.WCFListener](https://www.nuget.org/packages/LaserCatEyes.WCFListener/) from the package manager console:
```
PM> Install-Package LaserCatEyes.WCFListener
```

2. In ``Startup`` class ``ConfigureServices`` method inject add Endpoint Listener

#### To listen all HttpClients
```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        if (CurrentEnvironment.IsDevelopment()) 
        {
            //This is a debugging tool, you don't want to run it in production, right!?
            services.AddLaserCatEyesWCFListener(MY_APP_KEY_FROM_LASER_CAT_EYES_PORTAL);
        }
    }
```

```
    public class ChannelFactoryHandler<T> : IChannelFactoryHandler<T> where T : class
    {
        public ChannelFactoryHandler(ChannelFactory<T> channelFactory, IEndpointBehavior endpointBehavior)
        {
            if (CurrentEnvironment.IsDevelopment()) 
            {
                channelFactory.Endpoint.EndpointBehaviors.Add(endpointBehavior);
            }
        }
```
[Laser-Cat-Eyes web portal]: <https://portal.lasercateyes.com>
[Laser Cat Eyes]: <https://lasercateyes.com>

## Author

<img src="http://www.betalgo.com/img/logo-dark.png" width="10px"> Betalgo, mail@betalgo.com
