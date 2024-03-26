using System;
using System.Net.Http;
using System.Threading.Tasks;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LaserCatEyes.DataServiceSdk;

public class LaserCatEyesDataService : ILaserCatEyesDataService
{
    private const string AlgoronaClientId = "989C784C-2EB2-4666-8796-D7494EBB745D";

    private readonly HttpClient _client = HttpClientFactory.Create(new HttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    });

    private readonly Guid _deviceId;
    private readonly LaserCatEyesOptions _laserCatEyesOptions;
    private readonly LaserCatEyesSystemOptions _laserCatEyesSystemOptions;
    private readonly ILogger<LaserCatEyesDataService> _logger;
    private readonly bool _serviceReady;

    public LaserCatEyesDataService(IOptions<LaserCatEyesOptions> laserCatEyesOptions, IOptions<LaserCatEyesSystemOptions> laserCatEyesSystemOptions, ILogger<LaserCatEyesDataService> logger)
    {
        _logger = logger;
        try
        {
            _laserCatEyesOptions = laserCatEyesOptions.Value;
            _laserCatEyesSystemOptions = laserCatEyesSystemOptions.Value;

            if (string.IsNullOrEmpty(_laserCatEyesOptions.AppKey))
            {
                logger.LogWarning("LaserCatEyes AppKey is NULL!");
                return;
            }

            var deviceName = $"{Environment.MachineName}:{Environment.UserName}";
            _laserCatEyesOptions.DeviceUuid ??= Utilities.ToGuid(deviceName);

            SubAppUpdate subApp = new()
            {
                Device = new Device
                {
                    Name = deviceName,
                    OSVersion = Environment.OSVersion.Version.ToString(),
                    OperatingSystem = Environment.OSVersion.VersionString.Replace(Environment.OSVersion.Version.ToString(), ""),
                    UUID = _laserCatEyesOptions.DeviceUuid.ToString(),
                    UserFriendlyName = _laserCatEyesOptions.DeviceUserFriendlyName
                },
                OperatingSystem = Environment.OSVersion.VersionString.Replace(Environment.OSVersion.Version.ToString(), ""),
                Name = _laserCatEyesOptions.AppName ?? "Unknown",
                Environment = _laserCatEyesOptions.AspCoreEnvironment,
                Version = _laserCatEyesOptions.Version,
                BuildNumber = _laserCatEyesOptions.BuildNumber
            };

            _client.BaseAddress = new Uri(_laserCatEyesSystemOptions.BaseAddress);
            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaClientId, !string.IsNullOrEmpty(_laserCatEyesSystemOptions.AlgoronaClientId) ? _laserCatEyesSystemOptions.AlgoronaClientId : AlgoronaClientId);
            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaCulture, _laserCatEyesSystemOptions.AlgoronaCulture);
            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaDeviceUuid, _laserCatEyesOptions.DeviceUuid.ToString());
            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaAppKey, _laserCatEyesOptions.AppKey);

            var httpResponseMessage = Init(subApp).Result;
            _deviceId = httpResponseMessage.Content.ReadAsAsync<SubAppUpdateResponseModel>().Result.DeviceId;

            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaDeviceId, _deviceId.ToString());
            _serviceReady = true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "LaserCatEyes Error on initialization");
        }
    }


    public async Task<HttpResponseMessage> ReportTask(PackageData data)
    {
        if (!_serviceReady)
        {
            return null;
        }

        try
        {
            data.DeviceUuid = _laserCatEyesOptions.DeviceUuid;
            data.DeviceId = _deviceId;
            return await _client.PostAsJsonAsync(_laserCatEyesSystemOptions.Endpoints.DataSendPackage, data);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "LaserCatEyes Error on initialization");
            return null;
        }
    }


    public bool IsServiceReady()
    {
        return _serviceReady;
    }

    public void Report(PackageData data)
    {
        if (!_serviceReady)
        {
            return;
        }

        Task.Run(() => ReportTask(data)).Forget();
    }

    private async Task<HttpResponseMessage> Init(SubAppUpdate data)
    {
        return await _client.PutAsJsonAsync(_laserCatEyesSystemOptions.Endpoints.AppUpdateSubApp, data);
    }
}