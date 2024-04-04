
using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace LaserCatEyes.FunctionEndpointListener
{
    public static class LaserCatEyesServiceCollectionExtensions
    {
        public static IServiceCollection AddLaserCatEyesFunctionEndpointListener(this IServiceCollection services, Action<LaserCatEyesOptions> setupAction)
        {
            services.TryAddSingleton<IConfigureOptions<LaserCatEyesOptions>>(new ConfigureNamedOptions<LaserCatEyesOptions>(setupAction.Method.Name, setupAction));
            return AddLaserCatEyesFunctionEndpointListenerBase(services);
        }

        public static IServiceCollection AddLaserCatEyesFunctionEndpointListener(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.Configure<LaserCatEyesOptions>(configuration.GetSection(LaserCatEyesOptions.SettingKey));

            if (configuration.GetSection(LaserCatEyesSystemOptions.SettingKey) != null)
            {
                services.Configure<LaserCatEyesSystemOptions>(configuration.GetSection(LaserCatEyesSystemOptions.SettingKey));
            }

            return AddLaserCatEyesFunctionEndpointListenerBase(services);
        }

        public static IServiceCollection AddLaserCatEyesFunctionEndpointListener(this IServiceCollection services, string appKey)
        {
            services.TryAddSingleton(Options.Create(new LaserCatEyesOptions(appKey)));
            return AddLaserCatEyesFunctionEndpointListenerBase(services);
        }

        private static IServiceCollection AddLaserCatEyesFunctionEndpointListenerBase(IServiceCollection services)
        {
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            services.AddTransient<FunctionEndpointListenerMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseLaserCatEyesFunctionEndpointListenerMiddleware(this IApplicationBuilder services)
        {
            services.UseMiddleware<FunctionEndpointListenerMiddleware>();
            return services;
        }
    }
}
