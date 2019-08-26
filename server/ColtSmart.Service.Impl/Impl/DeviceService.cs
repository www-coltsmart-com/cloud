using ColtSmart.Data;
using ColtSmart.Entity;
using ColtSmart.Entity.Entities;
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
            //如果无法辨别当前用户，则默认返回空列表，防止信息泄露
            if (string.IsNullOrEmpty(userNo))
            {
                return new PagedResult<Device>()
                {
                    Result = null,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalCount = 0
                };
            }
            var users = await this.sqlExecutor.FindAsync<TUser>(new { UserNo = userNo });
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return new PagedResult<Device>()
                {
                    Result = null,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalCount = 0
                };
            }
            //拼接SQL字符串
            StringBuilder sqlBuilder = new StringBuilder();
            if (user.UserType != EUserType.Admin)//如果不是管理员，则只看到当前用户名下的设备清单
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
                UserOwn = user.UserNo,
                DeviceId = string.IsNullOrEmpty(deviceId) ? "%" : string.Format("{0}%", deviceId.Trim()),
                DeviceName = string.IsNullOrEmpty(deviceName) ? "%" : string.Format("{0}%", deviceName.Trim())
            };
            if (sqlBuilder.Length > 0) sqlBuilder.Insert(0, " WHERE ");
            sqlBuilder.Insert(0, "SELECT * FROM device");
            var results = await sqlExecutor.QueryPageAsync<Device>(sqlBuilder.ToString(), page, pageSize, param);
            return results.ToPagedResult();
        }

        public async Task<bool> Insert(Device device)
        {
            return await this.sqlExecutor.InsertAsync<Device>(device) > 0;
        }

        public async Task<bool> Update(Device device)
        {
            return await this.sqlExecutor.ExecuteAsync("update device set \"DeviceType\"=@DeviceType, \"IsGetway\" =@IsGetway, \"DeviceName\" =@DeviceName, \"IsOnline\" =@IsOnline, \"InDate\" =@InDate, \"UserOwn\" =@UserOwn, \"Gps\" =@Gps, \"Version\" =@Version, \"ComPortNum\" =@ComPortNum where \"DeviceId\"=@DeviceId", new
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
            }, commandType: System.Data.CommandType.Text) > 0;
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="id">设备唯一标识</param>
        /// <returns>影响行数</returns>
        public async Task<bool> Delete(int id)
        {
            var results = await this.sqlExecutor.FindAsync<Device>(new { Id = id });
            if (results == null || !results.Any()) return false;
            return await this.sqlExecutor.DeleteAsync(results.First()) > 0;
        }

        public async Task<bool> UpdateOnline(string deviceId, bool isOnline)
        {
            return await this.sqlExecutor.ExecuteAsync("update device set \"IsOnline\" =@IsOnline where \"DeviceId\"=@DeviceId", new
            {
                DeviceId = deviceId,
                IsOnline = isOnline
            }, System.Data.CommandType.Text) > 0;
        }

        public async Task<bool> UpdateDeviceNet(string deviceId, double netFlow)
        {
            return await this.sqlExecutor.InsertAsync<DeviceNet>(new DeviceNet
            {
                DeviceId = deviceId,
                NetFlow = netFlow
            }) > 0;
        }

        public async Task<int> GetDeviceCount(string userNo, bool isOnline = false)
        {
            //拼接SQL字符串
            StringBuilder sqlBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(userNo))
            {
                if (sqlBuilder.Length > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append("\"UserOwn\" = @UserOwn");
            }
            if (isOnline)
            {
                if (sqlBuilder.Length > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append("\"IsOnline\" = @IsOnline");
            }
            object param = new
            {
                UserOwn = userNo,
                IsOnline = isOnline
            };
            if (sqlBuilder.Length > 0) sqlBuilder.Insert(0, " WHERE ");
            sqlBuilder.Insert(0, "SELECT COUNT(1) FROM device");
            return await sqlExecutor.ExecuteScalarAsync<int>(sqlBuilder.ToString(), param);
        }
    }
}
