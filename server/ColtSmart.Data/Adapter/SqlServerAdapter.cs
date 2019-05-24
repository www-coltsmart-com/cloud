using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ColtSmart.Data.Adapter
{
    public class SqlServerAdapter : SqlAdapter
    {

        public override bool BulkInsert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, IList entityToInsert)
        {
            var tableName = tableInfo.TableName;
            using (var bulkCopy = new SqlBulkCopy(connection as SqlConnection, SqlBulkCopyOptions.Default, transaction as SqlTransaction))
            {
                bulkCopy.BulkCopyTimeout = commandTimeout ?? 30;
                bulkCopy.BatchSize = 0;
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(ToDataTable(entityToInsert, tableName, tableInfo.InsertColumns.Select(ci => ci.Property).ToList()).CreateDataReader());
            }

            return true;
        }

        public override async Task<bool> BulkInsertAsync(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, TableInfo tableInfo, IList entityToInsert)
        {
            var tableName = tableInfo.TableName;
            using (var bulkCopy = new SqlBulkCopy(connection as SqlConnection, SqlBulkCopyOptions.Default, transaction as SqlTransaction))
            {
                bulkCopy.BulkCopyTimeout = commandTimeout ?? 30;
                bulkCopy.BatchSize = 0;
                bulkCopy.DestinationTableName = tableName;
                await bulkCopy.WriteToServerAsync(ToDataTable(entityToInsert, tableName, tableInfo.InsertColumns.Select(ci => ci.Property).ToList()).CreateDataReader());
            }

            return true;
        }
    }
}
