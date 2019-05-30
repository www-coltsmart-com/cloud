using ColtSmart.Entity;
using System.Threading.Tasks;

namespace ColtSmart.Service
{
    public interface IDeviceService
    {
        Task<Device> GetDevice(string deviceId);

        Task<Device> GetDevice(string deviceId, string userNo);

        Task<PagedResult<Device>> GetDevices(int page, int pageSize, string deviceName);

        Task Delete(int id);

        Task Update(Device device);

        Task Insert(Device device);
    }
}
