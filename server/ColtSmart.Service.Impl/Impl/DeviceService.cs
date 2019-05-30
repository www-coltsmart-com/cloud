using ColtSmart.Data;
using ColtSmart.Entity;
using System.Linq;
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
    }
}
