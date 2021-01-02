using System;
using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.AspNetCore.Builder;
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
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            services.AddTransient<EndpointListenerMiddleware>();
            return services;
        }

        public static IServiceCollection AddLaserCatEyesEndpointListener(this IServiceCollection services, string appKey)
        {
            services.TryAddSingleton(Options.Create(new LaserCatEyesOptions(appKey)));
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