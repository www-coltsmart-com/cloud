using ColtSmart.Data;
using ColtSmart.Entity;
using System.Linq;

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
            var device= sqlExecutor.Find<Device>(new { DeviceId = deviceId }).FirstOrDefault();

            return device;
        }

        public Device GetDevice(string deviceId, string userNo)
        {
            var device = sqlExecutor.Find<Device>(new { DeviceId = deviceId, UserOwn=userNo }).FirstOrDefault();

            return device;
        }

        public void Insert(Device device)
        {
           // device.Id= sqlExecutor.GetId("device");
            sqlExecutor.Insert<Device>(device);
        }

        public void Update(Device device)
        {
            throw new System.NotImplementedException();
        }
    }
}
