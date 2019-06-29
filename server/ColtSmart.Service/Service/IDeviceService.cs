using ColtSmart.Entity;
using System.Threading.Tasks;

namespace ColtSmart.Service
{
    public interface IDeviceService
    {
        Task<Device> GetDevice(string deviceId);

        Task<Device> GetDevice(string deviceId, string userNo);
        /// <summary>
        /// 获取用户名下的设备列表
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页多少行</param>
        /// <param name="userNo">使用人</param>
        /// <param name="deviceId">设备编号</param>
        /// <param name="deviceName">设备名称</param>
        /// <returns>设备列表</returns>
        Task<PagedResult<Device>> GetDevices(int page, int pageSize, string userNo, string deviceId, string deviceName);

        Task Update(Device device);

        Task Insert(Device device);

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="id">设备唯一标识</param>
        /// <returns>影响行数</returns>
        Task<int> Delete(int id);

        Task UpdateOnline(string deviceId, bool isOnline);

        Task UpdateDeviceNet(string deviceId, double netFlow);

        Task<int> GetDeviceCount(string userNo, bool isOnline = false);
    }
}
