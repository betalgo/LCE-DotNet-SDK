using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LaserCatEyes.EndpointListener;

public class EndpointListenerMiddleware : IMiddleware
{
    private readonly bool _isServiceReady;
    private readonly ILaserCatEyesDataService _laserCatEyesDataService;

    public EndpointListenerMiddleware(ILaserCatEyesDataService laserCatEyesDataService, ILogger<EndpointListenerMiddleware> logger)
    {
        if (laserCatEyesDataService == null)
        {
            logger.LogWarning($"Couldn't bind {nameof(EndpointListenerMiddleware)} because {nameof(ILaserCatEyesDataService)} is null");
            return;
        }

        if (!laserCatEyesDataService.IsServiceReady())
        {
            logger.LogWarning($"Couldn't bind {nameof(EndpointListenerMiddleware)} because {nameof(ILaserCatEyesDataService)} was not ready");
            return;
        }

        _laserCatEyesDataService = laserCatEyesDataService;
        _isServiceReady = true;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context?.Request == null || !_isServiceReady)
        {
            await next(context);
            return;
        }

        var operationId = Guid.NewGuid();
#if NET6_0_OR_GREATER
        context.Request.EnableBuffering();
#else
        context.Request.EnableRewind();
#endif

        _laserCatEyesDataService.Report(PackageData.CreateRequestPackage(
            operationId,
            $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
            Utilities.HttpMethodStringToEnumConverter(context.Request.Method),
            context.Request.Headers?.SelectMany(r => r.Value.Select(value => new HeaderCouple(r.Key, value))).ToList(),
            await Utilities.ReadBodyStream(context.Request.Body),
            DateTime.UtcNow,
            context.Connection?.LocalIpAddress?.ToString(),
            context.Connection?.RemoteIpAddress?.ToString()
        ));

        var originalResponseBody = context.Response.Body;
        MemoryStream replacementResponseBody = new();
        context.Response.Body = replacementResponseBody;

        await next(context);

        replacementResponseBody.Position = 0;
        await replacementResponseBody.CopyToAsync(originalResponseBody);
        context.Response.Body = originalResponseBody;
        _laserCatEyesDataService.Report(PackageData.CreateResponsePackage(
            operationId,
            context.Response.StatusCode,
            context.Response.Headers.SelectMany(r => r.Value.Select(value => new HeaderCouple(r.Key, value))).ToList(),
            await Utilities.ReadBodyStream(replacementResponseBody),
            DateTime.UtcNow));

#if NET6_0_OR_GREATER
        await replacementResponseBody.DisposeAsync();
#else
        replacementResponseBody.Dispose();
#endif
    }
}