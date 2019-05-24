using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ColtSmart.Data
{
    public interface ISqlExecutor : ISqlBulkAction, IDisposable
    {
        #region Sync
        IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        IEnumerable<object> Query(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        T QueryFirst<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        object QueryFirst(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        T QueryFirstOrDefault<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        object QueryFirstOrDefault(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        T QuerySingle<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        object QuerySingle(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        T ExecuteScalar<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        object ExecuteScalar(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        int Execute(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        IDataReader ExecuteReader(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        int Insert<T>(T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<int> InsertAsync<T>(T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null);

        int Update<T>(T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<int> UpdateAsync<T>(T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null);

        int Delete<T>(T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<int> DeleteAsync<T>(T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null);

        int Delete<T>(object keys, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<int> DeleteAsync<T>(object keys, IDbTransaction transaction = null, int? commandTimeout = null);

        IEnumerable<T> Find<T>(object parameter, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<IEnumerable<T>> FindAsync<T>(object parameter, IDbTransaction transaction = null, int? commandTimeout = null);

        IPagedEnumerable<T> QueryPage<T>(string sql, int page, int pageSize, object parameters, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<IPagedEnumerable<T>> QueryPageAsync<T>(string sql, int page, int pageSize, object parameters, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);
        #endregion

        #region Async

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<IEnumerable<object>> QueryAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<T> QueryFirstAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<object> QueryFirstAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<object> QueryFirstOrDefaultAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<T> QuerySingleAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<object> QuerySingleAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<T> ExecuteScalarAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<object> ExecuteScalarAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<int> ExecuteAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null);


        #endregion

        #region basic
        DbTransaction BeginTransaction();
        DbTransaction BeginTransaction(IsolationLevel isolationLevel);
        //DbCommand CreateCommand();
        //DbParameter CreateParameter();
        string GetWhere(object paramter);
        #endregion
    }

}
