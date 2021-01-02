using System;
using System.Collections.Generic;

namespace LaserCatEyes.Domain.Models
{
    public class ResponsePackage
    {
        public ResponsePackage(Guid id, int? statusCode, List<string> headers, string body, DateTime? timeStamp)
        {
            Id = id;
            StatusCode = statusCode;
            Headers = headers;
            Body = body;
            TimeStamp = timeStamp ?? DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? StatusCode { get; set; }
        public List<string> Headers { get; set; }
        public string Body { get; set; }
    }
}