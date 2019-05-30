using ColtSmart.Entity;
using System.Threading.Tasks;

namespace ColtSmart.Service
{
    public interface IDeviceService
    {
        Task<Device> GetDevice(string deviceId);

        Task<Device> GetDevice(string deviceId, string userNo);

        Task Update(Device device);

        Task Insert(Device device);
    }
}
