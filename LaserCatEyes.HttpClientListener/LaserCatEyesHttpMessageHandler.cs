using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LaserCatEyes.Domain;

namespace LaserCatEyes.HttpClientListener
{
    public class LaserCatEyesHttpMessageHandler : DelegatingHandler
    {
        private readonly ILaserCatEyesDataService _laserCatEyesDataService;

        public LaserCatEyesHttpMessageHandler(ILaserCatEyesDataService laserCatEyesDataService)
        {
            _laserCatEyesDataService = laserCatEyesDataService ?? throw new ArgumentNullException(nameof(laserCatEyesDataService));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return await base.SendAsync(null, cancellationToken);
            }

            var operationId = Guid.NewGuid();
            _laserCatEyesDataService.Report(PackageDataHelper.RequestPackageDataFromHttpRequestMessage(operationId, request));

            var response = await base.SendAsync(request, cancellationToken);
            
            _laserCatEyesDataService.Report(PackageDataHelper.ResponsePackageDataFromHttpResponseMessage(operationId, response));
            return response;
        }
    }
}