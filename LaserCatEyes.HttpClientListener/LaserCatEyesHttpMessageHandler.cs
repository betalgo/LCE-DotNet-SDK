using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LaserCatEyes.Domain;
using Microsoft.Extensions.Logging;

namespace LaserCatEyes.HttpClientListener
{
    public class LaserCatEyesHttpMessageHandler : DelegatingHandler
    {
        private readonly ILaserCatEyesDataService _laserCatEyesDataService;
        private readonly bool _serviceReady;

        public LaserCatEyesHttpMessageHandler(ILaserCatEyesDataService laserCatEyesDataService, ILogger<LaserCatEyesHttpMessageHandler> logger)
        {
            if (laserCatEyesDataService == null)
            {
                logger.LogWarning($"Couldn't bind {nameof(LaserCatEyesHttpMessageHandler)} because {nameof(ILaserCatEyesDataService)} is null");
                return;
            }

            if (!laserCatEyesDataService.IsServiceReady())
            {
                logger.LogWarning($"Couldn't bind {nameof(LaserCatEyesHttpMessageHandler)} because {nameof(ILaserCatEyesDataService)} was not ready");
                return;
            }

            _laserCatEyesDataService = laserCatEyesDataService;
            _serviceReady = true;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null || !_serviceReady)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var operationId = Guid.NewGuid();
            _laserCatEyesDataService.Report(PackageDataHelper.RequestPackageDataFromHttpRequestMessage(operationId, request));

            var response = await base.SendAsync(request, cancellationToken);

            _laserCatEyesDataService.Report(PackageDataHelper.ResponsePackageDataFromHttpResponseMessage(operationId, response));
            return response;
        }
    }
}