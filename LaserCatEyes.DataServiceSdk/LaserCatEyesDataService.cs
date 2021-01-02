using System;
using System.Net.Http;
using System.Threading.Tasks;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Extensions.Options;

namespace LaserCatEyes.DataServiceSdk
{
    public class LaserCatEyesDataService : ILaserCatEyesDataService
    {
        private readonly HttpClient _client = HttpClientFactory.Create();
        private readonly Guid _deviceId;
        private readonly LaserCatEyesOptions _laserCatEyesOptions;
        private readonly LaserCatEyesSystemOptions _laserCatEyesSystemOptions;

        public LaserCatEyesDataService(IOptions<LaserCatEyesOptions> laserCatEyesOptions, IOptions<LaserCatEyesSystemOptions> laserCatEyesSystemOptions)
        {
            _laserCatEyesOptions = laserCatEyesOptions.Value;
            _laserCatEyesSystemOptions = laserCatEyesSystemOptions.Value;

            if (string.IsNullOrEmpty(_laserCatEyesOptions.AppKey))
            {
                throw new Exception("LaserCatEyes AppKey can not be null!");
            }

            var deviceName = $"{Environment.MachineName}:{Environment.UserName}";
            _laserCatEyesOptions.DeviceUuid ??= Utilities.ToGuid(deviceName);

            var subApp = new SubAppUpdate
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
            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaClientId, _laserCatEyesSystemOptions.AlgoronaClientId);
            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaCulture, _laserCatEyesSystemOptions.AlgoronaCulture);
            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaDeviceUuid, _laserCatEyesOptions.DeviceUuid.ToString());
            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaAppKey, _laserCatEyesOptions.AppKey);

            var httpResponseMessage = Init(subApp).Result;
            _deviceId = httpResponseMessage.Content.ReadAsAsync<SubAppUpdateResponseModel>().Result.DeviceId;

            _client.DefaultRequestHeaders.Add(Constants.Headers.AlgoronaDeviceId, _deviceId.ToString());
        }


        public async Task<HttpResponseMessage> ReportTask(PackageData data)
        {
            data.DeviceUuid = _laserCatEyesOptions.DeviceUuid;
            data.DeviceId = _deviceId;
            return await _client.PostAsJsonAsync(_laserCatEyesSystemOptions.Endpoints.DataSendPackage, data);
        }

        public void Report(PackageData data)
        {
            Task.Run(() => ReportTask(data)).Forget();
        }

        private async Task<HttpResponseMessage> Init(SubAppUpdate data)
        {
            return await _client.PutAsJsonAsync(_laserCatEyesSystemOptions.Endpoints.AppUpdateSubApp, data);
        }
    }
}