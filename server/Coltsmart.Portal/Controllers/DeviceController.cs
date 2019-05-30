﻿using ColtSmart.Entity;
using ColtSmart.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coltsmart.Portal.Controllers
{
    public class DeviceController : ApiController
    {
        private IDeviceService deviceService = null;

        public DeviceController(IDeviceService deviceService)
        {
            this.deviceService = deviceService;
        }

        [HttpGet]
        [Route("api/devices")]
        public async Task<PagedResult<Device>> Get(int page, int size, string devicename)
        {
            return await deviceService.GetDevices(page, size, devicename);
        }

        [HttpDelete]
        [Route("api/devices/{id}")]
        public void Delete(int id)
        {
            deviceService.Delete(id);
        }
    }
}
