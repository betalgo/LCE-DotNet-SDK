namespace LaserCatEyes.Domain.Models;

public class LaserCatEyesSystemOptions
{
    public static string SettingKey = "LaserCatEyesSystemOptions";

    public string BaseAddress { get; set; } = "https://data.lasercateyes.com/";
    public string AlgoronaClientId { get; set; }
    public string AlgoronaCulture { get; set; } = "en-US";
    public EndpointPaths Endpoints { get; set; } = new();

    public class EndpointPaths
    {
        public string DataSendPackage { get; set; } = "/api/Data/SendPackage";
        public string AppUpdateSubApp { get; set; } = "/api/App/UpdateSubApp";
    }
}