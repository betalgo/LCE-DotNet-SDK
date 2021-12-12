using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace LaserCatEyes.EndpointListener
{
    public class EndpointListenerMiddleware : IMiddleware
    {
        private readonly ILaserCatEyesDataService _laserCatEyesDataService;

        public EndpointListenerMiddleware(ILaserCatEyesDataService laserCatEyesDataService)
        {
            _laserCatEyesDataService = laserCatEyesDataService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context?.Request == null)
            {
                await next(context);
                return;
            }

            var operationId = Guid.NewGuid();

            context.Request.EnableBuffering();
            _laserCatEyesDataService.Report(PackageData.CreateRequestPackage(
                operationId,
                $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                Utilities.HttpMethodStringToEnumConverter(context.Request.Method),
                context.Request.Headers?.SelectMany(r => r.Value.Select(value => $"{r.Key}:{value}")).ToList(),
                await Utilities.ReadBodyStream(context.Request.Body),
                DateTime.UtcNow
            ));

            var originalResponseBody = context.Response.Body;
            await using var replacementResponseBody = new MemoryStream();
            context.Response.Body = replacementResponseBody;

            await next(context);

            replacementResponseBody.Position = 0;
            await replacementResponseBody.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;
           _laserCatEyesDataService.Report(PackageData.CreateResponsePackage(
                operationId,
                context.Response.StatusCode,
                context.Response.Headers.SelectMany(r => r.Value.Select(value => $"{r.Key}:{value}")).ToList(),
                await Utilities.ReadBodyStream(replacementResponseBody),
                DateTime.UtcNow));
        }
    }
}