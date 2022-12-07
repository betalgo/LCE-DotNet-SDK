using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace LaserCatEyes.AzureFunctionEndpointListener
{
    public class AzureFunctionEndpointListenerMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly bool _isServiceReady;
        private readonly ILaserCatEyesDataService? _laserCatEyesDataService;

        public AzureFunctionEndpointListenerMiddleware(ILaserCatEyesDataService? laserCatEyesDataService, ILogger<AzureFunctionEndpointListenerMiddleware>? logger)
        {
            if (laserCatEyesDataService == null)
            {
                logger.LogWarning($"Couldn't bind {nameof(AzureFunctionEndpointListenerMiddleware)} because {nameof(ILaserCatEyesDataService)} is null");
                return;
            }

            if (!laserCatEyesDataService.IsServiceReady())
            {
                logger.LogWarning($"Couldn't bind {nameof(AzureFunctionEndpointListenerMiddleware)} because {nameof(ILaserCatEyesDataService)} was not ready");
                return;
            }

            _laserCatEyesDataService = laserCatEyesDataService;
            _isServiceReady = true;
        }


        public async Task Invoke(FunctionContext functionContext, FunctionExecutionDelegate next)
        {
            var operationId = Guid.NewGuid();
            var requestContext = await functionContext.GetHttpRequestDataAsync();
            if (requestContext == null || !_isServiceReady)
            {
                await next(functionContext);
                return;
            }

            _laserCatEyesDataService!.Report(PackageData.CreateRequestPackage(
                operationId, requestContext.Url.OriginalString,
                Utilities.HttpMethodStringToEnumConverter(requestContext.Method),
                requestContext.Headers.SelectMany(r => r.Value.Select(value => $"{r.Key}:{value}")).ToList(),
                await Utilities.ReadBodyStream(requestContext.Body),
                DateTime.UtcNow,
                requestContext.GetClientIpn()?.ToString(), //doesn't work with localhost
                null
            ));

            await next(functionContext);

            var responseContext = functionContext.GetHttpResponseData();

            if (responseContext != null) //Couldn't find a way to get response if application throw an exception
            {
                await using var replacementResponseBody = new MemoryStream();
                var originalPosition = responseContext.Body.Position;
                responseContext.Body.Position = 0;
                await responseContext.Body.CopyToAsync(replacementResponseBody);
                responseContext.Body.Position = originalPosition;

                _laserCatEyesDataService!.Report(PackageData.CreateResponsePackage(
                    operationId,
                    (int?) responseContext.StatusCode,
                    responseContext.Headers.SelectMany(r => r.Value.Select(value => $"{r.Key}:{value}")).ToList(),
                    await Utilities.ReadBodyStream(replacementResponseBody),
                    DateTime.UtcNow));
            }
        }
    }
}