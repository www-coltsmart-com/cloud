using System.Data.Common;

namespace ColtSmart.Data
{
    public class DbOptions
    {
        public string ConnectionString { get; set; }

        public DbProviderFactory DbProviderFactory { get; set; }
    }
}
