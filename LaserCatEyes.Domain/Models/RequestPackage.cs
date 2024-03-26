using System;
using System.Collections.Generic;

namespace LaserCatEyes.Domain.Models;

public class RequestPackage(Guid id, string url, MethodType methodType, List<HeaderCouple> headers, string body, DateTime? timeStamp)
{
    public Guid Id { get; set; } = id;
    public DateTime TimeStamp { get; set; } = timeStamp ?? DateTime.UtcNow;
    public string Url { get; set; } = url;
    public List<HeaderCouple> HeaderCouples { get; set; } = headers;

    public string Body { get; set; } = body;
    public MethodType MethodType { get; set; } = methodType;
}

public class HeaderCouple(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
}