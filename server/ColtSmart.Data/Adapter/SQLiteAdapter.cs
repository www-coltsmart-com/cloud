using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ColtSmart.Data.Adapter
{
    public class SQLiteAdapter : SqlAdapter
    {
        
        public override bool BulkInsert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, IList entityToInsert)
        {
            bool isTrans = transaction == null;
            if (connection.State != ConnectionState.Open)
                connection.Open();
            transaction = transaction ?? connection.BeginTransaction();
            var count = entityToInsert.Count;
            if (count == 0 || tableInfo.InsertColumns.Count() == 0)
                return true;

            int batchSize = (int)Math.Floor(((double)999)/ tableInfo.InsertColumns.Count());
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
                catch(Exception ex)
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

        protected override string QueryLastId => "SELECT last_insert_rowid() id";
    }
}
