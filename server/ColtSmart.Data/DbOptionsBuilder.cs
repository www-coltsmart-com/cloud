using MySql.Data.MySqlClient;
using Npgsql;
using System.Data.Common;
using System.Data.SqlClient;

namespace ColtSmart.Data
{
    public class DbOptionsBuilder
    {
        private string ConnectionString { get; set; }

        private DbProviderFactory DbProviderFactory { get; set; }


        public DbOptions Build()
        {
            return new DbOptions()
            {
                DbProviderFactory = this.DbProviderFactory,
                ConnectionString = this.ConnectionString
            };
        }

        public DbOptionsBuilder UseSqlServer()
        {
            return UseDbProviderFactory(SqlClientFactory.Instance);
        }

        public DbOptionsBuilder UseMySql()
        {
            return UseDbProviderFactory(MySqlClientFactory.Instance);
        }


        public DbOptionsBuilder UsePostgre()
        {
            return UseDbProviderFactory(NpgsqlFactory.Instance);
        }

        public DbOptionsBuilder UseDbProviderFactory(DbProviderFactory dbProviderFactory)
        {
            this.DbProviderFactory = dbProviderFactory;
            return this;
        }

        public DbOptionsBuilder UseConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;
            return this;
        }
    }
}
