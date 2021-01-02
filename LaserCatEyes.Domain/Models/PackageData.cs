using System;
using System.Collections.Generic;

namespace LaserCatEyes.Domain.Models
{
    public class PackageData
    {
        public Guid Id { get; set; }
        public RequestPackage RequestPackage { get; set; }
        public ResponsePackage ResponsePackage { get; set; }
        public List<string> Tags { get; set; }
        public Guid? DeviceId { get; set; }
        public Guid? DeviceUuid { get; set; }
        public Guid? AppId { get; set; }

        public static PackageData CreateRequestPackage(Guid id, string url, MethodType methodType, List<string> headers, string body, DateTime? timeStamp)
        {
            return new PackageData
            {
                RequestPackage = new RequestPackage(id, url, methodType, headers, body, timeStamp)
            };
        }

        public static PackageData CreateResponsePackage(Guid id, int? statusCode, List<string> headers, string body, DateTime? timeStamp)
        {
            return new PackageData
            {
                ResponsePackage = new ResponsePackage(id, statusCode, headers, body, timeStamp)
            };
        }
    }
}