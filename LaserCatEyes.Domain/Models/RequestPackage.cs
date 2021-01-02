using System;
using System.Collections.Generic;

namespace LaserCatEyes.Domain.Models
{
    public class RequestPackage
    {
        public RequestPackage(Guid id, string url, MethodType methodType, List<string> headers, string body, DateTime? timeStamp)
        {
            Id = id;
            Url = url;
            MethodType = methodType;
            Headers = headers;
            Body = body;
            TimeStamp = timeStamp ?? DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Url { get; set; }
        public List<string> Headers { get; set; }
        public string Body { get; set; }
        public MethodType MethodType { get; set; }
    }
}