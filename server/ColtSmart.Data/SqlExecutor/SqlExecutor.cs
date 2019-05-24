using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ColtSmart.Data
{
    public class SqlExecutor : ISqlExecutor, IDisposable
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _dbProviderFactory;

        private DbConnection _connection;

        public DbConnection Connection
        {
            get
            {
                _connection = _connection ?? this._dbProviderFactory.CreateConnection();

                if (string.IsNullOrWhiteSpace(_connection.ConnectionString))
                {
                    _connection.ConnectionString = this._connectionString;
                }

                return _connection;
            }
        }

        public SqlExecutor(string connectionString, DbProviderFactory dbProviderFactory)
        {
            Check.ArgumentNotNull(dbProviderFactory, nameof(dbProviderFactory));
            Check.ArgumentNotNullOrEmpty(connectionString, nameof(connectionString));
            
            this._connectionString = connectionString;
            this._dbProviderFactory = dbProviderFactory;
        }

        public SqlExecutor(DbOptions options) : this(options.ConnectionString, options.DbProviderFactory)
        {
            Check.ArgumentNotNull(options, nameof(options));
        }

        #region ISqlExecutor

        public int Execute(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.Execute(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<int> ExecuteAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.ExecuteAsync(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.ExecuteScalar<T>(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public object ExecuteScalar(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.ExecuteScalar(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.ExecuteScalarAsync<T>(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<object> ExecuteScalarAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.ExecuteScalarAsync(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.Query<T>(sql, PrepareParameters(param), transaction, true, commandTimeout, commandType);
        }

        public IEnumerable<object> Query(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.Query(sql, PrepareParameters(param), transaction, true, commandTimeout, commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryAsync<T>(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<IEnumerable<object>> QueryAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryAsync(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public T QueryFirst<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryFirst<T>(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public object QueryFirst(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryFirst(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<T> QueryFirstAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryFirstAsync<T>(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<object> QueryFirstAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryFirstAsync(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryFirstOrDefault<T>(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public object QueryFirstOrDefault(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryFirstOrDefault(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryFirstOrDefaultAsync<T>(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<object> QueryFirstOrDefaultAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QueryFirstOrDefaultAsync(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public T QuerySingle<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QuerySingle(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public object QuerySingle(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QuerySingle(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<T> QuerySingleAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QuerySingleAsync<T>(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<object> QuerySingleAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.QuerySingleAsync(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.ExecuteReaderAsync(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        public IDataReader ExecuteReader(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.ExecuteReader(sql, PrepareParameters(param), transaction, commandTimeout, commandType);
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    if (_connection != null)
                    {
                        if (_connection.State == ConnectionState.Open)
                        {
                            _connection.Close();
                        }
                        _connection.Dispose();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~SqlExecutor() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }


        #endregion

        #region private
        private object PrepareParameters(object param)
        {
            if (param is DataParameters)
                return (param as DataParameters).ToDynamicParameters();
            return param;
        }

        #endregion

        #region ISqlBulkAction

        public bool BulkInsert<T>(IEnumerable<T> items,IDbTransaction transaction = null) 
        {
            return this.Connection.BulkInsert<T>(items, transaction);
        }

        public async Task<bool> BulkInsertAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) 
        {
            return await this.Connection.BulkInsertAsync<T>(items, transaction);
        }

        public int Insert<T>(T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.Insert<T>(entityToInsert, transaction, commandTimeout);
        }

        public async Task<int> InsertAsync<T>(T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await this.Connection.InsertAsync<T>(entityToInsert, transaction, commandTimeout);
        }

        public int Update<T>(T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.Update<T>(entityToUpdate, transaction, commandTimeout);
        }

        public async Task<int> UpdateAsync<T>(T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await this.Connection.UpdateAsync<T>(entityToUpdate, transaction, commandTimeout);
        }

        public int Delete<T>(T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.Delete<T>(entityToDelete, transaction, commandTimeout);
        }

        public async Task<int> DeleteAsync<T>(object keys, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await this.Connection.DeleteAsync<T>(keys, transaction, commandTimeout);
        }

        public int Delete<T>(object keys, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.Delete<T>(keys, transaction, commandTimeout);
        }

        public async Task<int> DeleteAsync<T>(T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await this.Connection.DeleteAsync<T>(entityToDelete, transaction, commandTimeout);
        }

        public IEnumerable<T> Find<T>(object parameter, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return this.Connection.Find<T>(parameter, transaction, commandTimeout);
        }

        public async Task<IEnumerable<T>> FindAsync<T>(object parameter, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await this.Connection.FindAsync<T>(parameter, transaction, commandTimeout);
        }

        public DbTransaction BeginTransaction()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();

            return this.Connection.BeginTransaction();
        }

        public DbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();

            return this.Connection.BeginTransaction(isolationLevel);
        }

        public DbCommand CreateCommand()
        {
            return this._dbProviderFactory.CreateCommand();
        }

        public DbParameter CreateParameter()
        {
            return this._dbProviderFactory.CreateParameter();
        }

        public IPagedEnumerable<T> QueryPage<T>(string sql, int page, int pageSize, object parameters, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Connection.QueryPage<T>(sql, page, pageSize, parameters, commandType, transaction, commandTimeout);
        }

        public async Task<IPagedEnumerable<T>> QueryPageAsync<T>(string sql, int page, int pageSize, object parameters, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await Connection.QueryPageAsync<T>(sql, page, pageSize, parameters, commandType, transaction, commandTimeout);
        }

        public string GetWhere(object parameter)
        {
            return Connection.GetWhere(parameter);
        }
        #endregion
    }
}
