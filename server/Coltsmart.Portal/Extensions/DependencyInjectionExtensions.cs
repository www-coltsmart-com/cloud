using Autofac;
using ColtSmart.Core;
using ColtSmart.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Coltsmart.NetCore.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterDbExecutor(this ContainerBuilder builder, Action<DbOptionsBuilder> dbOptionBuilder)
        {
            var dbOptions = new DbOptionsBuilder();
            dbOptionBuilder(dbOptions);
            builder.RegisterInstance<DbOptions>(dbOptions.Build());
            builder.RegisterType<SqlExecutor>().As<ISqlExecutor>();
        }

        public static ConfigurationVariables BuildConfigurationVariables(this IConfiguration configuration)
        {
            return new ConfigurationVariables()
            {
                ConnectionString = configuration["ConnectionString"]
            };


        }
    }
}
