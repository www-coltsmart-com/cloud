using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColtSmart.Data
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync();
        IEnumerable<TEntity> GetData(string qry, object parameters);
        Task<IEnumerable<TEntity>> GetDataAsync(string qry, object parameters);
        TEntity Find(object pksFields);
        Task<TEntity> FindAsync(object pksFields);
        int Add(TEntity entity);
        Task<int> AddAsync(TEntity entity);
        bool Add(IEnumerable<TEntity> entities);
        Task<bool> AddAsync(IEnumerable<TEntity> entities);
        int Remove(object key);
        Task<int> RemoveAsync(object key);
        int Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        int InstertOrUpdate(TEntity entity, object pks);
        Task<int> InstertOrUpdateAsync(TEntity entity, object pks);
        IEnumerable<TEntity> GetData(object filter);
        Task<IEnumerable<TEntity>> GetDataAsync(object filter);
    }
}
