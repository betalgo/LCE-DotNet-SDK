using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LaserCatEyes.AzureFunctionEndpointListener
{
    public static class LaserCatEyesServiceCollectionExtensions
    {
        public static IServiceCollection AddLaserCatEyesAzureFunctionEndpointListener(this IServiceCollection services, Action<LaserCatEyesOptions> setupAction)
        {
            services.TryAddSingleton<IConfigureOptions<LaserCatEyesOptions>>(new ConfigureNamedOptions<LaserCatEyesOptions>(setupAction.Method.Name, setupAction));
            return AddLaserCatAzureFunctionEyesEndpointListenerBase(services);
        }

        public static IServiceCollection AddLaserCatEyesAzureFunctionEndpointListener(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.Configure<LaserCatEyesOptions>(configuration.GetSection(LaserCatEyesOptions.SettingKey));

            if (configuration.GetSection(LaserCatEyesSystemOptions.SettingKey) != null)
            {
                services.Configure<LaserCatEyesSystemOptions>(configuration.GetSection(LaserCatEyesSystemOptions.SettingKey));
            }

            return AddLaserCatAzureFunctionEyesEndpointListenerBase(services);
        }

        public static IServiceCollection AddLaserCatAzureFunctionEyesEndpointListener(this IServiceCollection services, string appKey)
        {
            services.TryAddSingleton(Options.Create(new LaserCatEyesOptions(appKey)));
            return AddLaserCatAzureFunctionEyesEndpointListenerBase(services);
        }

        private static IServiceCollection AddLaserCatAzureFunctionEyesEndpointListenerBase(IServiceCollection services)
        {
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            services.AddTransient<AzureFunctionEndpointListenerMiddleware>();
            return services;
        }

        public static IFunctionsWorkerApplicationBuilder UseLaserCatEyesEndpointListenerMiddleware(this IFunctionsWorkerApplicationBuilder services)
        {
            services.UseMiddleware<AzureFunctionEndpointListenerMiddleware>();
            return services;
        }
    }
}