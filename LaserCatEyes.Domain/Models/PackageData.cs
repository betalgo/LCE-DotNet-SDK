using System;
using System.Collections.Generic;

namespace LaserCatEyes.Domain.Models;

public class PackageData
{
    public Guid Id { get; set; }
    public RequestPackage RequestPackage { get; set; }
    public ResponsePackage ResponsePackage { get; set; }
    public List<string> Tags { get; set; }
    public Guid? DeviceId { get; set; }
    public Guid? DeviceUuid { get; set; }
    public Guid? AppId { get; set; }
    public string LocalIpAddress { get; set; }
    public string RemoteIpAddress { get; set; }

    public static PackageData CreateRequestPackage(Guid id, string url, MethodType methodType, List<HeaderCouple> headers, string body, DateTime? timeStamp, string localIpAddress, string remoteIpAddress)
    {
        return new PackageData
        {
            Id = id,
            RequestPackage = new RequestPackage(id, url, methodType, headers, body, timeStamp),
            LocalIpAddress = localIpAddress,
            RemoteIpAddress = remoteIpAddress
        };
    }

    public static PackageData CreateResponsePackage(Guid id, int? statusCode, List<HeaderCouple> headers, string body, DateTime? timeStamp)
    {
        return new PackageData
        {
            Id = id,
            ResponsePackage = new ResponsePackage(id, statusCode, headers, body, timeStamp)
        };
    }
}