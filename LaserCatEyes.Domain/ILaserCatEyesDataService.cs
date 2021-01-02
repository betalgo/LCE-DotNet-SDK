using System.Net.Http;
using System.Threading.Tasks;
using LaserCatEyes.Domain.Models;

namespace LaserCatEyes.Domain
{
    public interface ILaserCatEyesDataService
    {
        void Report(PackageData data);
        Task<HttpResponseMessage> ReportTask(PackageData data);
    }
}