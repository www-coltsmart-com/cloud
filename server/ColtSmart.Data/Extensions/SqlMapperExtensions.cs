using ColtSmart.Data.Adapter;
using System;
using System.Collections.Concurrent;
using System.Data;

namespace ColtSmart.Data
{
    public static partial class SqlMapperExtensions
    {
        
        private static readonly ISqlAdapter DefaultAdapter = new SqlServerAdapter();
        private static readonly ConcurrentDictionary<string, ISqlAdapter> AdapterDictionary
            = new ConcurrentDictionary<string, ISqlAdapter>
            {
                ["sqlconnection"] = new SqlServerAdapter(),
                ["sqliteconnection"] = new SQLiteAdapter(),
                ["mysqlconnection"] = new MySqlAdapter(),
                ["npgsqlconnection"] = new PostgreAdapter(),
                //["sqlceconnection"] = new SqlCeServerAdapter(),
                //["fbconnection"] = new FirebirdAdapter()
            };

       
        private static string CleanSqlAdapterName(string name) => name.ToLowerInvariant();

        private static string GetSqlAdapterName<TConnection>() where TConnection : IDbConnection, new()
            => CleanSqlAdapterName(typeof(TConnection).Name);

        private static string GetSqlAdapterName(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            return CleanSqlAdapterName(connection.GetType().Name);
        }

        private static ISqlAdapter GetFormatter(IDbConnection connection)
        {
            var name = GetSqlAdapterName(connection);

            return !AdapterDictionary.ContainsKey(name)
                ? DefaultAdapter
                : AdapterDictionary[name];
        }

        /// <summary>
        /// Configure the specified name to resolve to a custom SQL adapter.
        /// </summary>
        /// <param name="name">The name to assign the adapter.</param>
        /// <param name="adapter">An <see cref="ISqlAdapter"/> to register under <paramref name="name" />.</param>
        /// <exception cref="NullReferenceException">if <paramref name="name"/> or <paramref name="adapter"/> is null.</exception>
        internal static void AddSqlAdapter(string name, ISqlAdapter adapter)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));

            AdapterDictionary.AddOrUpdate(CleanSqlAdapterName(name), adapter, (_name, oldAdapter) => adapter);
        }

        /// <summary>
        /// Configure the specified connection to resolve to a custom SQL adapter.
        /// </summary>
        /// <param name="connection">An <see cref="IDbConnection"/> for which the concrete type will be mapped to the specified adapter by name.</param>
        /// <param name="adapter">An <see cref="ISqlAdapter"/> to register under <paramref name="connection" />.</param>
        /// <exception cref="NullReferenceException">if <paramref name="connection"/> or <paramref name="adapter"/> is null.</exception>
        internal static void AddSqlAdapter(IDbConnection connection, ISqlAdapter adapter)
            => AddSqlAdapter(GetSqlAdapterName(connection), adapter);

        /// <summary>
        /// Configure the specified connection to resolve to a custom SQL adapter.
        /// </summary>
        /// <typeparam name="TConnection">An <see cref="IDbConnection"/> type which will be mapped to the specified adapter by its <see cref="System.Reflection.MemberInfo.Name"/>.</typeparam>
        /// <param name="adapter">An <see cref="ISqlAdapter"/> to register under <typeparamref name="TConnection" />.</param>
        /// <exception cref="NullReferenceException">if <paramref name="adapter"/> is null.</exception>
        internal static void AddSqlAdapter<TConnection>(ISqlAdapter adapter) where TConnection : IDbConnection, new()
            => AddSqlAdapter(GetSqlAdapterName<TConnection>(), adapter);
    }
}
