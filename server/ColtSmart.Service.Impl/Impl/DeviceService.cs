using ColtSmart.Data;
using ColtSmart.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColtSmart.Service.Impl
{
    public class DeviceService : IDeviceService
    {
        private readonly ISqlExecutor sqlExecutor;

        public DeviceService(DbOptions dbOptions)
        {
            this.sqlExecutor = new SqlExecutor(dbOptions);
        }

        public async Task<Device> GetDevice(string deviceId)
        {
            var device = await this.sqlExecutor.FindAsync<Device>(new { DeviceId = deviceId });
            return device.FirstOrDefault();
        }

        public async Task<Device> GetDevice(string deviceId, string userNo)
        {
            var device = await this.sqlExecutor.FindAsync<Device>(new { DeviceId = deviceId, UserOwn = userNo });
            return device.FirstOrDefault();
        }
        /// <summary>
        /// 获取当前用户的设备列表，管理员可以查看所有设备，注册用户只能看到自己名下的设备
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="userNo">使用人</param>
        /// <param name="deviceId">设备编号</param>
        /// <param name="deviceName">设备名称</param>
        /// <returns>设备列表</returns>
        public async Task<PagedResult<Device>> GetDevices(int page, int pageSize, string userNo, string deviceId, string deviceName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(userNo))
            {
                if (sqlBuilder.Length > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append("\"UserOwn\" = @UserOwn");
            }
            if (!string.IsNullOrEmpty(deviceId))
            {
                if (sqlBuilder.Length > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append("\"DeviceId\" LIKE @DeviceId");
            }
            if (!string.IsNullOrEmpty(deviceName))
            {
                if (sqlBuilder.Length > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append("\"DeviceName\" LIKE @DeviceName");
            }
            object param = new
            {
                UserOwn = string.IsNullOrEmpty(userNo) ? "" : userNo.Trim(),
                DeviceId = string.IsNullOrEmpty(deviceId) ? "%" : string.Format("{0}%", deviceId.Trim()),
                DeviceName = string.IsNullOrEmpty(deviceName) ? "%" : string.Format("{0}%", deviceName.Trim())
            };
            if (sqlBuilder.Length > 0) sqlBuilder.Insert(0, " WHERE ");
            sqlBuilder.Insert(0, "SELECT * FROM device");
            var results = await sqlExecutor.QueryPageAsync<Device>(sqlBuilder.ToString(), page, pageSize, param);
            return results.ToPagedResult();
        }

        public async Task Insert(Device device)
        {
            await this.sqlExecutor.InsertAsync<Device>(device);
        }

        public async Task Update(Device device)
        {
            await this.sqlExecutor.ExecuteAsync("upddate device set \"DeviceType\"=@DeviceType, \"IsGetway\" =@IsGetway, \"DeviceName\" =@DeviceName, \"IsOnline\" =@IsOnline, \"InDate\" =@InDate, \"UserOwn\" =@UserOwn, \"Gps\" =@Gps, \"Version\" =@Version, \"ComPortNum\" =@ComPortNum where DeviceId=@DeviceId", new
            {
                #region
                DeviceId = device.DeviceId,
                DeviceType = device.DeviceType,
                IsGetway = device.IsGetway,
                DeviceName = device.DeviceName,
                IsOnline = device.IsOnline,
                InDate = device.InDate,
                UserOwn = device.UserOwn,
                Gps = device.Gps,
                Version = device.Version,
                ComPortNum = device.ComPortNum
                #endregion
            }, commandType: System.Data.CommandType.Text);
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="id">设备唯一标识</param>
        /// <returns>影响行数</returns>
        public async Task<int> Delete(int id)
        {
            var results = await this.sqlExecutor.FindAsync<Device>(new { Id = id });
            var device = results.FirstOrDefault();
            if (device != null)
            {
               return await this.sqlExecutor.DeleteAsync(device);
            }
            return 0;
        }
    }
}
