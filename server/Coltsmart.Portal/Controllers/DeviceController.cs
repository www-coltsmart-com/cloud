using ColtSmart.Entity;
using ColtSmart.Service;
using ColtSmart.Service.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coltsmart.Portal.Controllers
{
    public class DeviceController : ApiController
    {
        private IDeviceService deviceService = null;
        private IUserService userService = null;

        public DeviceController(IDeviceService deviceService,IUserService userService)
        {
            this.deviceService = deviceService;
            this.userService = userService;
        }
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="deviceId">设备编号</param>
        /// <param name="deviceName">设备名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/devices")]
        public async Task<PagedResult<Device>> Get(int page, int size, string userNo, string deviceId, string deviceName)
        {
            return await deviceService.GetDevices(page, size, userNo, deviceId, deviceName);
        }

        [HttpDelete]
        [Route("api/devices/{id}")]
        public async Task<int> Delete(int id)
        {
            return await deviceService.Delete(id);
        }
    }
}
