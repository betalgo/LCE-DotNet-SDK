using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Xml;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;

namespace LaserCatEyes.WCFListener
{
    public static class PackageDataHelper
    {
        public static PackageData RequestPackageDataFromHttpRequestMessage(Guid id, ref Message request)
        {
            using var buffer = request.CreateBufferedCopy(int.MaxValue);
            var document = GetDocument(buffer.CreateMessage());
            request = buffer.CreateMessage();

            return PackageData.CreateRequestPackage(
                id,
                request.Headers.Action,
                MethodType.POST,
                new List<string>(),
                document.OuterXml,
                DateTime.UtcNow,
                null,
                null);
        }

        public static PackageData ResponsePackageDataFromHttpResponseMessage(PackageData data, ref Message message)
        {
            var httpResponseMessageProperty = message.Properties.Values.OfType<HttpResponseMessageProperty>().FirstOrDefault();
            var getHttpResponseMessageMethod = typeof(HttpResponseMessageProperty).GetRuntimeMethod("get_HttpResponseMessage", Type.EmptyTypes);
            var response = (HttpResponseMessage) getHttpResponseMessageMethod.Invoke(httpResponseMessageProperty, null);

            using var buffer = message.CreateBufferedCopy(int.MaxValue);
            var document = GetDocument(buffer.CreateMessage());
            message = buffer.CreateMessage();

            data.RequestPackage.Url = response.RequestMessage.RequestUri.ToString();
            data.RequestPackage.Headers = response.RequestMessage.Headers.SelectMany(r => r.Value.Select(value => $"{r.Key}:{value}")).ToList();
            data.RequestPackage.MethodType = Utilities.HttpMethodStringToEnumConverter(response.RequestMessage.Method.Method);

            data.ResponsePackage = PackageData.CreateResponsePackage(
                data.Id,
                (int) response.StatusCode, response.Headers.SelectMany(r => r.Value.Select(value => $"{r.Key}:{value}")).ToList(),
                document.OuterXml,
                DateTime.UtcNow).ResponsePackage;

            return data;
        }

        private static XmlDocument GetDocument(Message request)
        {
            var document = new XmlDocument();
            using var memoryStream = new MemoryStream();

            var writer = XmlWriter.Create(memoryStream);
            request.WriteMessage(writer);
            writer.Flush();
            memoryStream.Position = 0;

            document.Load(memoryStream);

            return document;
        }
    }
}