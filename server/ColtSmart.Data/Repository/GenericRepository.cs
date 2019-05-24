using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColtSmart.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ISqlExecutor sqlExecutor;

        public GenericRepository(DbOptions options)
        {
            this.sqlExecutor = new SqlExecutor(options);
        }

        public int Add(TEntity entity)
        {
            return sqlExecutor.Insert<TEntity>(entity);
        }

        public bool Add(IEnumerable<TEntity> entities)
        {
            return sqlExecutor.BulkInsert<TEntity>(entities.ToArray());
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            return await sqlExecutor.InsertAsync<TEntity>(entity);
        }

        public async Task<bool> AddAsync(IEnumerable<TEntity> entities)
        {
            return await sqlExecutor.BulkInsertAsync<TEntity>(entities.ToArray());
        }

        public IEnumerable<TEntity> All()
        {
            return sqlExecutor.Find<TEntity>(null);
        }

        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await sqlExecutor.FindAsync<TEntity>(null);
        }

        public void Dispose()
        {
            sqlExecutor.Dispose();
        }

        public TEntity Find(object pksFields)
        {
            return sqlExecutor.Find<TEntity>(pksFields).FirstOrDefault();
        }

        public async Task<TEntity> FindAsync(object pksFields)
        {
            return await Task.Run(() => sqlExecutor.FindAsync<TEntity>(pksFields).Result.FirstOrDefault());
        }

        public IEnumerable<TEntity> GetData(string qry, object parameters)
        {
            return sqlExecutor.Query<TEntity>(qry, parameters);
        }

        public IEnumerable<TEntity> GetData(object filter)
        {
            return sqlExecutor.Find<TEntity>(filter);
        }

        public async Task<IEnumerable<TEntity>> GetDataAsync(string qry, object parameters)
        {
            return await sqlExecutor.QueryAsync<TEntity>(qry, parameters);
        }

        public async Task<IEnumerable<TEntity>> GetDataAsync(object filter)
        {
            return await sqlExecutor.FindAsync<TEntity>(filter);
        }

        public int InstertOrUpdate(TEntity entity, object pks)
        {
            int result = 0;

            var entityInTable = Find(pks);

            if (entityInTable == null)
            {
                result = Add(entity);
            }
            else
            {
                result = Update(entity);
            }

            return result;
        }

        public async Task<int> InstertOrUpdateAsync(TEntity entity, object pks)
        {
            return await Task.Run(() => InstertOrUpdate(entity, pks));
        }

        public int Remove(object key)
        {
           return sqlExecutor.Delete<TEntity>(key);
        }

        public async Task<int> RemoveAsync(object key)
        {
           return await sqlExecutor.DeleteAsync<TEntity>(key);
        }

        public int Update(TEntity entity)
        {
            return sqlExecutor.Update<TEntity>(entity);
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            return await sqlExecutor.UpdateAsync<TEntity>(entity);
        }
    }
}
