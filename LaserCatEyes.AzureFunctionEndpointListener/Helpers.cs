using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace LaserCatEyes.AzureFunctionEndpointListener
{
    public static class Helpers
    {
        public static IPAddress? GetClientIpn(this HttpRequestData request)
        {
            IPAddress? result = null;
            if (request.Headers.TryGetValues("X-Forwarded-For", out var values))
            {
                var ipn = values.FirstOrDefault()?.Split(new[] {','}).FirstOrDefault()?.Split(new char[] {':'}).FirstOrDefault();
                IPAddress.TryParse(ipn, out result);
            }

            return result;
        }
    }
}