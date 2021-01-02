using System;
using LaserCatEyes.Domain;
using Microsoft.Extensions.Http;

namespace LaserCatEyes.HttpClientListener.DotNetStandard
{
    public class LaserCatEyesHttpMessageHandlerFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly ILaserCatEyesDataService _laserCatEyesDataService;

        public LaserCatEyesHttpMessageHandlerFilter(ILaserCatEyesDataService laserCatEyesDataService)
        {
            _laserCatEyesDataService = laserCatEyesDataService ?? throw new ArgumentNullException(nameof(laserCatEyesDataService));
        }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            return builder =>
            {
                next(builder);
                builder.AdditionalHandlers.Insert(0, new LaserCatEyesHttpMessageHandler(_laserCatEyesDataService));
            };
        }
    }
}