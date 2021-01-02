namespace LaserCatEyes.Domain.Models
{
    public class SubAppUpdate
    {
        public string OperatingSystem { get; set; }
        public string Name { get; set; }
        public string Environment { get; set; }
        public string Version { get; set; }
        public Device Device { get; set; }
        public string BuildNumber { get; set; }
    }

    public class Device
    {
        public string Name { get; set; }
        public string UserFriendlyName { get; set; }
        public string UUID { get; set; }
        public string OperatingSystem { get; set; }
        public string OSVersion { get; set; }
    }
}