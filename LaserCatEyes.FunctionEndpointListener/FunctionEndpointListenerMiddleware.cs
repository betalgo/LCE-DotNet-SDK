using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace LaserCatEyes.FunctionEndpointListener;

public class FunctionEndpointListenerMiddleware : IFunctionsWorkerMiddleware
{
    private readonly bool _isServiceReady;
    private readonly ILaserCatEyesDataService? _laserCatEyesDataService;
    
    public FunctionEndpointListenerMiddleware(ILaserCatEyesDataService? laserCatEyesDataService, ILogger<FunctionEndpointListenerMiddleware> logger)
    {
        if (laserCatEyesDataService == null)
        {
            logger.LogWarning($"Couldn't bind {nameof(FunctionEndpointListenerMiddleware)} because {nameof(ILaserCatEyesDataService)} is null");
            return;
        }

        if (!laserCatEyesDataService.IsServiceReady())
        {
            logger.LogWarning($"Couldn't bind {nameof(FunctionEndpointListenerMiddleware)} because {nameof(ILaserCatEyesDataService)} was not ready");
            return;
        }

        _laserCatEyesDataService = laserCatEyesDataService;
        _isServiceReady = true;
    }


    public async Task Invoke(FunctionContext functionContext, FunctionExecutionDelegate next)
    {
        var operationId = Guid.NewGuid();

        try
        {
            var context = await functionContext.GetHttpRequestDataAsync();
            if (context == null || !_isServiceReady)
            {
                await next(functionContext);
                return;
            }

            _laserCatEyesDataService!.Report(PackageData.CreateRequestPackage(
                operationId, context.Url.OriginalString,
                Utilities.HttpMethodStringToEnumConverter(context.Method),
                context.Headers?.SelectMany(r => r.Value.Select(value => new HeaderCouple(r.Key, value))).ToList(),
                await Utilities.ReadBodyStream(context.Body),
                DateTime.UtcNow,
                null,
                null
            ));

            await next(functionContext);
        }
        finally
        {
            var response = functionContext.GetHttpResponseData();
            await using var replacementResponseBody = new MemoryStream();
            if (response != null)
            {
                var originalPosition = response.Body.Position;
                response.Body.Position = 0;
                await response.Body.CopyToAsync(replacementResponseBody);
                response.Body.Position = originalPosition;
            }
            
            _laserCatEyesDataService!.Report(PackageData.CreateResponsePackage(
                operationId,
                (int?)response?.StatusCode,
                response?.Headers.SelectMany(r => r.Value.Select(value => new HeaderCouple(r.Key, value))).ToList(),
                await Utilities.ReadBodyStream(replacementResponseBody),
                DateTime.UtcNow));
        }
    }
}