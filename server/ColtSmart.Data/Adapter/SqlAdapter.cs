using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ColtSmart.Data.Adapter
{
    public abstract class SqlAdapter : ISqlAdapter
    {
        protected IPartsQryGenerator PartsQryGenerator
        {
            get;
            set;
        }

        public SqlAdapter()
        {
            PartsQryGenerator = new PartsQryGenerator();
        }

        #region ISqlAdapter

        public virtual bool BulkInsert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, IList entityToInsert)
        {
            return false;
        }

        public virtual async Task<bool> BulkInsertAsync(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, IList entityToInsert)
        {
            return await Task.Run(() => { return BulkInsert(connection, transaction, commandTimeout, tableInfo, entityToInsert); });
        }

        public virtual int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, object entityToInsert)
        {
            //var cmd = new StringBuilder(PartsQryGenerator.InsertQuery(tableInfo));
            //return connection.Execute(cmd.ToString(), entityToInsert, transaction, commandTimeout);

            var cmd = new StringBuilder(PartsQryGenerator.InsertQuery(tableInfo)).Append(QueryLastId);
            var multi = connection.QueryMultiple(cmd.ToString(), entityToInsert, transaction, commandTimeout);

            var first = multi.Read().FirstOrDefault();
            if (first == null || first.id == null) return 0;

            var id = (int)first.id;
            var propertyInfos = tableInfo.KeyColumns?.Select(f => f.Property).ToArray();
            if (propertyInfos?.Length == 0) return id;

            var idProperty = propertyInfos[0];
            idProperty.SetValue(entityToInsert, Convert.ChangeType(id, idProperty.PropertyType), null);

            return id;
        }

        public virtual async Task<int> InsertAsync(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, object entityToInsert)
        {
            return await Task.Run(() => Insert(connection, transaction, commandTimeout, tableInfo, entityToInsert));
        }

        public virtual int Delete(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, object entityToInsert)
        {
            var cmd = new StringBuilder(PartsQryGenerator.DeleteQuery(tableInfo));
            return connection.Execute(cmd.ToString(), entityToInsert, transaction, commandTimeout);
        }

        public virtual async Task<int> DeleteAsync(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, object entityToInsert)
        {
            var cmd = new StringBuilder(PartsQryGenerator.DeleteQuery(tableInfo));
            return await connection.ExecuteAsync(cmd.ToString(), entityToInsert, transaction, commandTimeout);
        }

        public virtual int Update(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, object entityToUpdate, IEnumerable<string> columnsToUpdate)
        {
            var cmd = new StringBuilder(PartsQryGenerator.UpdateQuery(tableInfo, columnsToUpdate));
            return connection.Execute(cmd.ToString(), entityToUpdate, transaction, commandTimeout);
        }

        public virtual async Task<int> UpdateAsync(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, object entityToUpdate, IEnumerable<string> columnsToUpdate)
        {
            var cmd = new StringBuilder(PartsQryGenerator.UpdateQuery(tableInfo, columnsToUpdate));
            return await connection.ExecuteAsync(cmd.ToString(), entityToUpdate, transaction, commandTimeout);
        }


        public IEnumerable<T> Find<T>(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, object parameter)
        {
            var filtersPksFields = new List<string>();
            if (parameter != null)
            {
                if (parameter is Dictionary<string, object>)
                    filtersPksFields = (parameter as Dictionary<string, object>).Keys.ToList();
                else
                    filtersPksFields = parameter.GetType().GetProperties().Select(a => a.Name).ToList();
            }          

            var cmd = new StringBuilder(PartsQryGenerator.GetQuery(tableInfo, filtersPksFields));
            return connection.Query<T>(cmd.ToString(), parameter, transaction, true, commandTimeout);
        }

        public async Task<IEnumerable<T>> FindAsync<T>(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, object parameter)
        {
            var filtersPksFields = new List<string>();
            if (parameter != null)
            {
                if (parameter is Dictionary<string, object>)
                    filtersPksFields = (parameter as Dictionary<string, object>).Keys.ToList();
                else
                    filtersPksFields = parameter.GetType().GetProperties().Select(a => a.Name).ToList();
            }
            var cmd = new StringBuilder(PartsQryGenerator.GetQuery(tableInfo, filtersPksFields));
            return await connection.QueryAsync<T>(cmd.ToString(), parameter, transaction, commandTimeout);
        }

        #endregion

        #region protected

        protected DataTable ToDataTable(IEnumerable data, string tableName, IList<PropertyInfo> properties)
        {
            var dataTable = new DataTable(tableName);
            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop.Name);
            }

            var typeCasts = new Type[properties.Count];
            for (var i = 0; i < properties.Count; i++)
            {
                var isEnum = properties[i].PropertyType.IsEnum;
                if (isEnum)
                {
                    typeCasts[i] = Enum.GetUnderlyingType(properties[i].PropertyType);
                }
                else
                {
                    typeCasts[i] = null;
                }
            }

            foreach (var item in data)
            {
                var values = new object[properties.Count];
                for (var i = 0; i < properties.Count; i++)
                {
                    var value = properties[i].GetValue(item, null);
                    var castToType = typeCasts[i];
                    values[i] = castToType == null ? value : Convert.ChangeType(value, castToType);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public string GetWhere(object parameter)
        {
            var filtersPksFields = new List<string>();
            if (parameter != null)
            {
                if (parameter is Dictionary<string, object>)
                    filtersPksFields = (parameter as Dictionary<string, object>).Keys.ToList();
                else
                    filtersPksFields = parameter.GetType().GetProperties().Select(a => a.Name).ToList();
            }

            return PartsQryGenerator.EscapeWhereList(filtersPksFields);
        }

        protected virtual string QueryLastId { get { return "select SCOPE_IDENTITY() id"; } }
        #endregion
    }
}
