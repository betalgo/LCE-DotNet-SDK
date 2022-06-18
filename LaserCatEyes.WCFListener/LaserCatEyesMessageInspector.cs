using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Extensions.Logging;

namespace LaserCatEyes.WCFListener
{
    public class LaserCatEyesMessageInspector : IClientMessageInspector
    {
        private readonly ILaserCatEyesDataService _laserCatEyesDataService;
        private readonly bool _serviceReady;

        public LaserCatEyesMessageInspector(ILaserCatEyesDataService laserCatEyesDataService, ILogger<LaserCatEyesMessageInspector> logger = null)
        {
            if (laserCatEyesDataService == null)
            {
                logger?.LogWarning($"Couldn't bind {nameof(LaserCatEyesMessageInspector)} because {nameof(ILaserCatEyesDataService)} is null");
                return;
            }

            if (!laserCatEyesDataService.IsServiceReady())
            {
                logger?.LogWarning($"Couldn't bind {nameof(LaserCatEyesMessageInspector)} because {nameof(ILaserCatEyesDataService)} was not ready");
                return;
            }

            _laserCatEyesDataService = laserCatEyesDataService;
            _serviceReady = true;
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (!_serviceReady)
            {
                return null!;
            }

            var operationId = Guid.NewGuid();
            //TODO Update this whenever backend start to accept updating request object
            //laserCatEyesDataService!.Report(PackageDataHelper.RequestPackageDataFromHttpRequestMessage(operationId, ref request));
            return PackageDataHelper.RequestPackageDataFromHttpRequestMessage(operationId, ref request);
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (!_serviceReady)
            {
                return;
            }

            _laserCatEyesDataService!.Report(PackageDataHelper.ResponsePackageDataFromHttpResponseMessage((PackageData) correlationState, ref reply));
        }
    }
}