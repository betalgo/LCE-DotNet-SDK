using System;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using LaserCatEyes.DataServiceSdk.DotNetStandard;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace LaserCatEyes.WCFListener
{
    public static class WCFListenerServiceCollectionExtensions
    {
        public static IServiceCollection AddLaserCatEyesWCFListener(this IServiceCollection services, string appKey)
        {
            services.TryAddSingleton(Options.Create(new LaserCatEyesOptions(appKey)));
            return AddLaserCatEyesWCFListenerBase(services);
        }

        public static IServiceCollection AddLaserCatEyesWCFListener(this IServiceCollection services, Action<LaserCatEyesOptions> setupAction)
        {
            services.TryAddSingleton<IConfigureOptions<LaserCatEyesOptions>>(new ConfigureNamedOptions<LaserCatEyesOptions>(setupAction.Method.Name, setupAction));
            return AddLaserCatEyesWCFListenerBase(services);
        }

        private static IServiceCollection AddLaserCatEyesWCFListenerBase(IServiceCollection services)
        {
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            services.AddTransient<IEndpointBehavior, LaserCatEyesEndpointBehaviour>();
            services.AddTransient<IClientMessageInspector, LaserCatEyesMessageInspector>();
            return services;
        }
    }
}