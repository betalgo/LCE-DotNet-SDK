using System;
using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace LaserCatEyes.EndpointListener
{
    public static class LaserCatEyesServiceCollectionExtensions
    {
        public static IServiceCollection AddLaserCatEyesEndpointListener(this IServiceCollection services, Action<LaserCatEyesOptions> setupAction)
        {
            services.TryAddSingleton<IConfigureOptions<LaserCatEyesOptions>>(new ConfigureNamedOptions<LaserCatEyesOptions>(setupAction.Method.Name, setupAction));
            return AddLaserCatEyesEndpointListenerBase(services);
        }

        public static IServiceCollection AddLaserCatEyesEndpointListener(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.Configure<LaserCatEyesOptions>(configuration.GetSection(LaserCatEyesOptions.SettingKey));
            
            if (configuration.GetSection(LaserCatEyesSystemOptions.SettingKey) != null)
            {
                services.Configure<LaserCatEyesSystemOptions>(configuration.GetSection(LaserCatEyesSystemOptions.SettingKey));
            }
            return AddLaserCatEyesEndpointListenerBase(services);
        }

        public static IServiceCollection AddLaserCatEyesEndpointListener(this IServiceCollection services, string appKey)
        {
            services.TryAddSingleton(Options.Create(new LaserCatEyesOptions(appKey)));
            return AddLaserCatEyesEndpointListenerBase(services);
        }

        private static IServiceCollection AddLaserCatEyesEndpointListenerBase(IServiceCollection services)
        {
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            services.AddTransient<EndpointListenerMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseLaserCatEyesEndpointListenerMiddleware(this IApplicationBuilder services)
        {
            services.UseMiddleware<EndpointListenerMiddleware>();
            return services;
        }
    }
}