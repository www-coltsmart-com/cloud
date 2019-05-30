using ColtSmart.Data;
using ColtSmart.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColtSmart.Service.Impl
{
    public class DeviceService: IDeviceService
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

        public async Task<PagedResult<Device>> GetDevices(int page, int pageSize, string deviceName)
        {
            StringBuilder sqlBuilder = new StringBuilder("SELECT * FROM device");
            object param = null;
            if (!string.IsNullOrEmpty(deviceName))
            {
                sqlBuilder.Append(" WHERE \"DeviceName\" like @DeviceName");
                param = new { DeviceName = string.Format("{0}%", deviceName.Trim()) };
            }
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
                DeviceType=device.DeviceType,
                IsGetway=device.IsGetway,
                DeviceName=device.DeviceName,
                IsOnline=device.IsOnline,
                InDate=device.InDate,
                UserOwn=device.UserOwn,
                Gps=device.Gps,
                Version=device.Version,
                ComPortNum=device.ComPortNum
                #endregion
            }, commandType: System.Data.CommandType.Text);
        }

        public async Task Delete(int id)
        {
            var results = await this.sqlExecutor.FindAsync<Device>(new { Id = id });
            var device = results.FirstOrDefault();
            if (device != null)
            {
                await this.sqlExecutor.DeleteAsync(device);
            }
        }
    }
}
