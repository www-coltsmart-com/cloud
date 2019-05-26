using ColtSmart.Entity;

namespace ColtSmart.Service
{
    public interface IDeviceService
    {
        Device GetDevice(string deviceId);

        Device GetDevice(string deviceId, string userNo);

        void Update(Device device);

        void Insert(Device device);
    }
}
