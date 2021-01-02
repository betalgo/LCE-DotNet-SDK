using System;

namespace LaserCatEyes.Domain.Models
{
    public class LaserCatEyesOptions
    {
        public LaserCatEyesOptions(string appKey)
        {
            AppKey = appKey;
        }

        public LaserCatEyesOptions()
        {
        }

        public string AppKey { get; set; }
        public string DeviceUserFriendlyName { get; set; } = $"{Environment.MachineName}:{Environment.UserName}:{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";
        public string Version { get; set; } = "0";
        public string BuildNumber { get; set; } = "0.0.0.0";
        public Guid? DeviceUuid { get; set; }
        public string AppName { get; set; } = Environment.GetEnvironmentVariable("IISEXPRESS_SITENAME");
        public string AspCoreEnvironment { get; set; } = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}