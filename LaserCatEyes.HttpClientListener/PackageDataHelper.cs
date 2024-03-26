using System;
using System.Linq;
using System.Net.Http;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;

namespace LaserCatEyes.HttpClientListener
{
    public static class PackageDataHelper
    {
        public static PackageData RequestPackageDataFromHttpRequestMessage(Guid id, HttpRequestMessage request)
        {
            return PackageData.CreateRequestPackage(
                id,
                request.RequestUri?.ToString(),
                Utilities.HttpMethodStringToEnumConverter(request.Method.Method),
                request.Headers.SelectMany(r => r.Value.Select(value => new HeaderCouple(r.Key, value))).ToList(),
                request.Content?.ReadAsStringAsync().Result,
                DateTime.UtcNow,
                null,
                null);
        }

        public static PackageData ResponsePackageDataFromHttpResponseMessage(Guid id, HttpResponseMessage response)
        {
            return PackageData.CreateResponsePackage(
                id,
                (int)response.StatusCode, response.Headers.SelectMany(r => r.Value.Select(value => new HeaderCouple(r.Key, value))).ToList(),
                response.Content.ReadAsStringAsync().Result,
                DateTime.UtcNow);
        }
    }
}