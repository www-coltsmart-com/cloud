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

            //var connectionString = this.GetConnection();

            //using (var con = new NpgsqlConnection(connectionString))
            //{
            //    var device = await con.FindAsync<Device>(new { DeviceId = deviceId});

            //    return device.FirstOrDefault();
            //}
        }

        public async Task<Device> GetDevice(string deviceId, string userNo)
        {
            var device = await this.sqlExecutor.FindAsync<Device>(new { DeviceId = deviceId, UserOwn = userNo });

            return device.FirstOrDefault();
            //var connectionString = this.GetConnection();

            //using (var con = new NpgsqlConnection(connectionString))
            //{
            //    var device = await con.FindAsync<Device>(new { DeviceId = deviceId, UserOwn = userNo });

            //    return device.FirstOrDefault();
            //}
        }

        public async Task Insert(Device device)
        {
            await this.sqlExecutor.InsertAsync<Device>(device);

            //var connectionString = this.GetConnection();

            //using (var con = new NpgsqlConnection(connectionString))
            //{
            //    await con.InsertAsync<Device>(device);
            //}
        }

        public void Update(Device device)
        {
            throw new System.NotImplementedException();
        }
    }
}
