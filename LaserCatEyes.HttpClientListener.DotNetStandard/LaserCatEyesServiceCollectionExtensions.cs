using System;
using LaserCatEyes.DataServiceSdk.DotNetStandard;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace LaserCatEyes.HttpClientListener.DotNetStandard
{
    public static class HttpClientListenerServiceCollectionExtensions
    {
        public static IServiceCollection AddLaserCatEyesHttpClientListener(this IServiceCollection services, string appKey)
        {
            services.TryAddSingleton(Options.Create(new LaserCatEyesOptions(appKey)));
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            services.Replace(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, LaserCatEyesHttpMessageHandlerFilter>());
            return services;
        }

        public static IServiceCollection AddLaserCatEyesHttpClientListener(this IServiceCollection services, Action<LaserCatEyesOptions> setupAction)
        {
            services.TryAddSingleton<IConfigureOptions<LaserCatEyesOptions>>(new ConfigureNamedOptions<LaserCatEyesOptions>(setupAction.Method.Name, setupAction));
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            services.Replace(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, LaserCatEyesHttpMessageHandlerFilter>());
            return services;
        }
    }
}