using System;
using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace LaserCatEyes.HttpClientListener
{
    public static class HttpClientListenerServiceCollectionExtensions
    {
        public static IServiceCollection AddLaserCatEyesHttpClientListener(this IServiceCollection services, string appKey,bool listenAllHttpClients =true)
        {
            services.TryAddSingleton(Options.Create(new LaserCatEyesOptions(appKey)));
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            if (listenAllHttpClients)
            {
                services.Replace(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, LaserCatEyesHttpMessageHandlerFilter>());
            }
            return services;
        }

        public static IServiceCollection AddLaserCatEyesHttpClientListener(this IServiceCollection services, Action<LaserCatEyesOptions> setupAction, bool listenAllHttpClients = true)
        {
            services.TryAddSingleton<IConfigureOptions<LaserCatEyesOptions>>(new ConfigureNamedOptions<LaserCatEyesOptions>(setupAction.Method.Name, setupAction));
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            if (listenAllHttpClients)
            {
                services.Replace(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, LaserCatEyesHttpMessageHandlerFilter>());
            }
            return services;
        }
    }
}