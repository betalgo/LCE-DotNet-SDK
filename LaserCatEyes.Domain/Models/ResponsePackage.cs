using System;
using System.Collections.Generic;

namespace LaserCatEyes.Domain.Models;

public class ResponsePackage(Guid id, int? statusCode, List<HeaderCouple> headerCouples, string body, DateTime? timeStamp)
{
    public Guid Id { get; set; } = id;
    public DateTime TimeStamp { get; set; } = timeStamp ?? DateTime.UtcNow;
    public int? StatusCode { get; set; } = statusCode;
    public List<HeaderCouple> HeaderCouples { get; set; } = headerCouples;
    public string Body { get; set; } = body;
}