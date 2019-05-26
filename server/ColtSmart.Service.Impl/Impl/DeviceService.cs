using ColtSmart.Data;
using ColtSmart.Entity;

namespace ColtSmart.Service.Impl
{
    public class DeviceService: IDeviceService
    {
        private readonly ISqlExecutor sqlExecutor;

        public DeviceService(ISqlExecutor sqlExecutor)
        {
            this.sqlExecutor = sqlExecutor;
        }

        public Device GetDevice(string deviceId)
        {
            throw new System.NotImplementedException();
        }

        public Device GetDevice(string deviceId, string userNo)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(Device device)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Device device)
        {
            throw new System.NotImplementedException();
        }
    }
}
