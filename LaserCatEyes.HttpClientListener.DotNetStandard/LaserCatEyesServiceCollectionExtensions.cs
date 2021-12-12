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
        public static IServiceCollection AddLaserCatEyesHttpClientListener(this IServiceCollection services, string appKey, bool listenAllHttpClients = true)
        {
            services.TryAddSingleton(Options.Create(new LaserCatEyesOptions(appKey)));
            return AddLaserCatEyesHttpClientListenerBase(services, listenAllHttpClients);
        }

        public static IServiceCollection AddLaserCatEyesHttpClientListener(this IServiceCollection services, Action<LaserCatEyesOptions> setupAction, bool listenAllHttpClients = true)
        {
            services.TryAddSingleton<IConfigureOptions<LaserCatEyesOptions>>(new ConfigureNamedOptions<LaserCatEyesOptions>(setupAction.Method.Name, setupAction));
            return AddLaserCatEyesHttpClientListenerBase(services, listenAllHttpClients);
        }

        private static IServiceCollection AddLaserCatEyesHttpClientListenerBase(IServiceCollection services, bool listenAllHttpClients)
        {
            services.TryAddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
            services.AddTransient<LaserCatEyesHttpMessageHandler>();
            if (listenAllHttpClients)
            {
                services.ConfigureAll<HttpClientFactoryOptions>(options =>
                {
                    options.HttpMessageHandlerBuilderActions.Add(builder => { builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<LaserCatEyesHttpMessageHandler>()); });
                });
            }

            return services;
        }
    }
}