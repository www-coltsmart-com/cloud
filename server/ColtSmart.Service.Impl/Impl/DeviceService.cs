using ColtSmart.Data;
using ColtSmart.Entity;
using ColtSmart.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColtSmart.Service.Impl.Impl
{
    public class DeviceService : IDeviceService
    {
        private readonly ISqlExecutor sqlExecutor = null;

        public DeviceService(ISqlExecutor sqlExecutor)
        {
            this.sqlExecutor = sqlExecutor;
        }

        public PagedResult<Device> GetDevices(int page, int pageSize, string deviceName)
        {
            StringBuilder sqlBuilder = new StringBuilder("SELECT * FROM device");
            object param = null;
            if (!string.IsNullOrEmpty(deviceName))
            {
                sqlBuilder.Append(" WHERE \"DeviceName\" like @DeviceName");
                param = new { DeviceName = string.Format("{0}%", deviceName.Trim()) };
            }
            return sqlExecutor.QueryPage<Device>(sqlBuilder.ToString(), page, pageSize, param).ToPagedResult();
        }

        public int DeleteDevice(int id)
        {
            Device device = sqlExecutor.Find<Device>(new { Id = id }).FirstOrDefault();
            if (device != null)
            {
                return sqlExecutor.Delete<Device>(device);
            }
            return 0;
        }
    }
}
