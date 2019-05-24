using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ColtSmart.Data.Adapter
{
    public class MySqlAdapter : SqlAdapter
    {

        public MySqlAdapter() : base()
        {
            PartsQryGenerator = new MySqlPartsQryGenerator();
        }

        public override int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, object entityToInsert)
        {
            var cmd = new StringBuilder(PartsQryGenerator.InsertQuery(tableInfo));
            connection.Execute(cmd.ToString(), entityToInsert, transaction, commandTimeout);
            var r = connection.Query("Select LAST_INSERT_ID() id", transaction: transaction, commandTimeout: commandTimeout);

            var id = r.First().id;
            if (id == null) return 0;
            var propertyInfos = tableInfo.KeyColumns?.Select(f => f.Property).ToArray();
            if (propertyInfos?.Length == 0) return Convert.ToInt32(id);

            var idp = propertyInfos[0];
            idp.SetValue(entityToInsert, Convert.ChangeType(id, idp.PropertyType), null);

            return Convert.ToInt32(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="tableInfo"></param>
        /// <param name="entityToInsert"></param>
        public override bool BulkInsert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, IList entityToInsert)
        {
            int batchSize = 1000;
            bool isTrans = transaction == null;
            if (connection.State != ConnectionState.Open)
                connection.Open();
            transaction = transaction ?? connection.BeginTransaction();
            var count = entityToInsert.Count;

            var batch = (int)Math.Ceiling(((double)count) / batchSize);
            var sqlpre = $"insert into { PartsQryGenerator.EscapeTableName(tableInfo)} ({PartsQryGenerator.EscapeColumnList(tableInfo.InsertColumns)}) values ";

            int last = count - ((batch - 1) * batchSize);
            var paramLength = (batch == 1 ? last : batchSize);
            var paramList = new List<string>();
            for (var b = 0; b < paramLength; b++)
            {
                paramList.Add($"({PartsQryGenerator.EscapeParameters(tableInfo.InsertColumns, b.ToString())})");
            }
            var bathSql = sqlpre + string.Join(",", paramList);
            var lastSql = sqlpre + string.Join(",", paramList.Take(last));
            for (var i = 0; i < batch; i++)
            {

                var parameters = new DynamicParameters();
                var exLength = (i * batchSize);
                var length = (i == batch - 1) ? last : batchSize;

                for (var p = 0; p < length; p++)
                {
                    foreach (var column in tableInfo.InsertColumns)
                    {
                        parameters.Add(column.ColumnName + p, column.Property.GetValue(entityToInsert[exLength + p], null));
                    }
                }

                try
                {
                    var sql = (i == batch - 1) ? lastSql : bathSql;
                    connection.Execute(sql, parameters, transaction, commandTimeout);
                }
                catch (Exception ex)
                {
                    if (isTrans)
                        transaction.Rollback();
                    throw ex;
                }
            }

            if (isTrans)
                transaction.Commit();

            if (connection.State == ConnectionState.Open && isTrans)
                connection.Close();

            return true;
        }
    }
}
