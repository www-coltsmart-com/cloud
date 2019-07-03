using ColtSmart.Data;
using ColtSmart.Entity;
using ColtSmart.Entity.Entities;
using ColtSmart.Service.Service;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ColtSmart.Service.Impl
{
    public class GoodsService : IGoodsService
    {
        private readonly ISqlExecutor sqlExecutor;

        public GoodsService(DbOptions dbOptions)
        {
            this.sqlExecutor = new SqlExecutor(dbOptions);
        }

        public async Task<PagedResult<Goods>> GetGoods(int page, int pageSize, string name)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(name))
            {
                if (sqlBuilder.Length > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append("\"Name\" LIKE @Name");
            }
            object param = new
            {
                Name = name
            };
            if (sqlBuilder.Length > 0) sqlBuilder.Insert(0, " WHERE ");
            sqlBuilder.Insert(0, "SELECT * FROM goods");
            var results = await sqlExecutor.QueryPageAsync<Goods>(sqlBuilder.ToString(), page, pageSize, param);
            return results.ToPagedResult();
        }

        public async Task<IEnumerable<GoodsAttr>> GetGoodsAttributes(int goodsId)
        {
            return await sqlExecutor.FindAsync<GoodsAttr>(new { GoodsId = goodsId });
        }

        public async Task<IEnumerable<GoodsAttach>> GetGoodsAttachments(int goodsId)
        {
            return await sqlExecutor.FindAsync<GoodsAttach>(new { GoodsId = goodsId });
        }

        public async Task<int> Insert(Goods goods)
        {
            return await sqlExecutor.InsertAsync(goods);
        }

        public async Task<int> Update(Goods goods)
        {
            return await sqlExecutor.ExecuteAsync("update goods set \"Name\"=@Name,\"Picture\"=@Picture,\"Info\"=@Info,\"Description\"=@Description,\"DisplayOrder\"=@DisplayOrder,\"Status\"=@Status where id=@id", new
            {
                Name = goods.Name,
                Picture = goods.Picture,
                Info = goods.Info,
                Description = goods.Description,
                DisplayOrder = goods.DisplayOrder,
                Status = goods.Status
            }, System.Data.CommandType.Text);
        }

        public async Task<int> Delete(int id)
        {
            var results = await sqlExecutor.FindAsync<Goods>(new { id = id });
            var result = results.FirstOrDefault();
            if (result != null)
            {
                return await sqlExecutor.DeleteAsync(result);
            }
            else
            {
                return 0;
            }
        }
    }
}
