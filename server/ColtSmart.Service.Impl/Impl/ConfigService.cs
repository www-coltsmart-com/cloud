using ColtSmart.Data;
using ColtSmart.Entity;
using ColtSmart.Entity.Entities;
using ColtSmart.Service.Service;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ColtSmart.Service.Impl
{
    public class ConfigService : IConfigService
    {
        private readonly ISqlExecutor sqlExecutor;

        public ConfigService(DbOptions dbOptions)
        {
            this.sqlExecutor = new SqlExecutor(dbOptions);
        }

        public async Task<Config> GetConfig(string key)
        {
            var results = await sqlExecutor.FindAsync<Config>(new { Key = key });
            return results.FirstOrDefault();
        }

        public async Task<PagedResult<Config>> GetConfigs(int page, int pageSize, string name)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(name))
            {
                if (sqlBuilder.Length > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append("\"Name\" LIKE @Name");
            }
            object param = new
            {
                Name = string.Format("%{0}%", name)
            };
            if (sqlBuilder.Length > 0) sqlBuilder.Insert(0, " WHERE ");
            sqlBuilder.Insert(0, "SELECT * FROM config");
            var results = await sqlExecutor.QueryPageAsync<Config>(sqlBuilder.ToString(), page, pageSize, param);
            return results.ToPagedResult();
        }

    }
}
