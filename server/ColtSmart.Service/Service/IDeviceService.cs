using ColtSmart.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColtSmart.Service.Service
{
    public interface IDeviceService
    {
        PagedResult<Device> GetDevices(int page, int pageSize, string deviceName);

        int DeleteDevice(int id);
    }
}
