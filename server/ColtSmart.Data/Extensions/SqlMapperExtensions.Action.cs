using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ColtSmart.Data
{
    public static partial class SqlMapperExtensions
    {
        #region Insert Queries
        /// <summary>
        /// Inserts an entity into table "Ts" and returns identity id or number of inserted rows if inserting a list.
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert, can be list of entities</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>the entity to insert or the list of entities</returns>
        public static int Insert<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) 
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return adapter.Insert(connection, transaction, commandTimeout, tinfo, entityToInsert);
        }

        /// <summary>
        /// Inserts an entity into table "Ts"
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if the entity was inserted</returns>
        public static async Task<int> InsertAsync<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) 
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return await adapter.InsertAsync(connection, transaction, commandTimeout, tinfo, entityToInsert);
        }
        #endregion

        #region Update Queries
        /// <summary>
        /// Updates entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static int Update<T>(this IDbConnection connection, T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) 
        {
            return connection.Update(entityToUpdate, null, transaction, commandTimeout);
        }

        /// <summary>
        /// Updates entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="columnsToUpdate">Columns to be updated</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static int Update<T>(this IDbConnection connection, T entityToUpdate, IEnumerable<string> columnsToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) 
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);

            return adapter.Update(connection, transaction, commandTimeout, tinfo, entityToUpdate, columnsToUpdate);
        }

        /// <summary>
        /// Updates entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static async Task<int> UpdateAsync<T>(this IDbConnection connection, T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) 
        {
            return await connection.UpdateAsync(entityToUpdate, null, transaction, commandTimeout);
        }

        /// <summary>
        /// Updates entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="columnsToUpdate">Columns to be updated</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static async Task<int> UpdateAsync<T>(this IDbConnection connection, T entityToUpdate, IEnumerable<string> columnsToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) 
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);

            return await adapter.UpdateAsync(connection, transaction, commandTimeout, tinfo, entityToUpdate, columnsToUpdate);

        }

        #endregion

        #region Delete
        /// <summary>
        /// Delete entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if not found</returns>
        public static int Delete<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null) 
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return adapter.Delete(connection, transaction, commandTimeout, tinfo, entityToDelete);
        }

        /// <summary>
        /// Delete entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if not found</returns>
        public static async Task<int> DeleteAsync<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null) 
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return await adapter.DeleteAsync(connection, transaction, commandTimeout, tinfo, entityToDelete);
        }

        /// <summary>
        /// Delete entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if not found</returns>
        public static int Delete<T>(this IDbConnection connection, object keys, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return adapter.Delete(connection, transaction, commandTimeout, tinfo, keys);
        }

        /// <summary>
        /// Delete entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if not found</returns>
        public static async Task<int> DeleteAsync<T>(this IDbConnection connection, object keys, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return await adapter.DeleteAsync(connection, transaction, commandTimeout, tinfo, keys);
        }
        #endregion


        #region Bulk Insert
        /// <summary>
        /// Bulk insert a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entityToInsert"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        public static bool BulkInsert<T>(this IDbConnection connection, IEnumerable<T> entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) 
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return adapter.BulkInsert(connection, transaction, commandTimeout, tinfo, entityToInsert.ToList());
        }

        /// <summary>
        /// Bulk insert a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entityToInsert"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        public static async Task<bool> BulkInsertAsync<T>(this IDbConnection connection, IEnumerable<T> entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return await adapter.BulkInsertAsync(connection, transaction, commandTimeout, tinfo, entityToInsert.ToList());
        }
        #endregion

        #region Query Queries
        /// <summary>
        /// Query entitires
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert, can be list of entities</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>the entity to insert or the list of entities</returns>
        public static IEnumerable<T> Find<T>(this IDbConnection connection, object parameter, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return adapter.Find<T>(connection, transaction, commandTimeout, tinfo, parameter);
        }

        /// <summary>
        /// Query entitires
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if the entity was inserted</returns>
        public static async Task<IEnumerable<T>> FindAsync<T>(this IDbConnection connection, object parameter, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var type = typeof(T);
            var adapter = GetFormatter(connection);
            var tinfo = TableInfo.TableInfoCache(type);
            return await adapter.FindAsync<T>(connection, transaction, commandTimeout, tinfo, parameter);
        }
        #endregion


        public static IPagedEnumerable<T> QueryPage<T>(this IDbConnection connection, string sql, int page, int pageSize, object parameters, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var result = new PagedList<T>();

            int count = 0;
            int first = (page - 1) * pageSize + 1;
            int last = page * pageSize;

            var reader = connection.ExecuteReader(sql, parameters, transaction, commandTimeout, commandType);
            var RowParser = reader.GetRowParser<T>(typeof(T), 0, reader.FieldCount);
            while (reader.Read())
            {
                count++;
                if (count >= first && last >= count)
                {
                    result.Add(RowParser(reader));
                }
            }
            result.PageSize = pageSize;
            result.CurrentPage = page;
            result.TotalCount = count;
            return result;
        }

        public static async Task<IPagedEnumerable<T>> QueryPageAsync<T>(this IDbConnection connection, string sql, int page, int pageSize, object parameters, CommandType? commandType = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await Task.Run<IPagedEnumerable<T>>(() => QueryPage<T>(connection, sql, page, pageSize, parameters, commandType, transaction, commandTimeout));
        }

        public static string GetWhere(this IDbConnection connection, object parameter)
        {
            var adapter = GetFormatter(connection);
            return adapter.GetWhere(parameter);
        }
    }
}
