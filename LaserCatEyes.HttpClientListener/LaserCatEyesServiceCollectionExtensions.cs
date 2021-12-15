using System;
using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace LaserCatEyes.HttpClientListener
{
    public static class HttpClientListenerServiceCollectionExtensions
    {
        public static IServiceCollection AddLaserCatEyesHttpClientListener(this IServiceCollection services, LaserCatEyesOptions options, bool listenAllHttpClients = true)
        {
            services.TryAddSingleton(options);
            return AddLaserCatEyesHttpClientListenerBase(services, listenAllHttpClients);
        }

        public static IServiceCollection AddLaserCatEyesHttpClientListener(this IServiceCollection services, bool listenAllHttpClients = true)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.Configure<LaserCatEyesOptions>(configuration.GetSection(LaserCatEyesOptions.SettingKey));
            return AddLaserCatEyesHttpClientListenerBase(services, listenAllHttpClients);
        }

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
            services.AddSingleton<ILaserCatEyesDataService, LaserCatEyesDataService>();
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