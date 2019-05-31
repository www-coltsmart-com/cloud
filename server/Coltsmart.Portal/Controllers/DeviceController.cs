using ColtSmart.Entity;
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
        public async Task<PagedResult<Device>> Get(int page, int size, string deviceId, string deviceName)
        {
            //TODO:添加权限控制
            string userNo = "";

            //判断当前用户是管理员或普通用户
            //如是管理员，则将userNo置空，表示所有设备列表均可见
            //否则，将userNo保留原值，表示只对名下设备列表可见

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
