using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ColtSmart.Data
{
    public interface ISqlBulkAction
    {
        bool BulkInsert<T>(IEnumerable<T> items, IDbTransaction transaction = null);
        Task<bool> BulkInsertAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null);
    }
}
