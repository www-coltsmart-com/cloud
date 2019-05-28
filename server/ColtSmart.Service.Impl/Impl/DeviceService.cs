using ColtSmart.Data;
using ColtSmart.Entity;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace ColtSmart.Service.Impl
{
    public class DeviceService: IDeviceService
    {
        private readonly ISqlExecutor sqlExecutor;
        private readonly IConfiguration configuration;

        public DeviceService(ISqlExecutor sqlExecutor, IConfiguration configuration)
        {
            this.sqlExecutor = sqlExecutor;
            this.configuration = configuration;
        }

        public async Task<Device> GetDevice(string deviceId)
        {
            var connectionString = this.GetConnection();

            using (var con = new NpgsqlConnection(connectionString))
            {
                var device = await con.FindAsync<Device>(new { DeviceId = deviceId});

                return device.FirstOrDefault();
            }
        }

        public async Task<Device> GetDevice(string deviceId, string userNo)
        {
            var connectionString = this.GetConnection();

            using (var con = new NpgsqlConnection(connectionString))
            {
                var device = await con.FindAsync<Device>(new { DeviceId = deviceId, UserOwn = userNo });

                return device.FirstOrDefault();
            }
        }

        public async Task Insert(Device device)
        {
            var connectionString = this.GetConnection();

            using (var con = new NpgsqlConnection(connectionString))
            {
              await  con.InsertAsync<Device>(device);
            }
        }

        public void Update(Device device)
        {
            throw new System.NotImplementedException();
        }

        public string GetConnection()
        {
            var connection = configuration.GetSection("ConnectionString").Value;
            return connection;
        }
    }
}
