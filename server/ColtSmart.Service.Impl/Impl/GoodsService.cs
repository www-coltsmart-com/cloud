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
                Name = string.Format("{0}%", name ?? "")
            };
            if (sqlBuilder.Length > 0) sqlBuilder.Insert(0, " WHERE ");
            sqlBuilder.Insert(0, "SELECT * FROM goods");
            var results = await sqlExecutor.QueryPageAsync<Goods>(sqlBuilder.ToString(), page, pageSize, param);
            return results.ToPagedResult();
        }

        public async Task<Goods> GetGoods(int id)
        {
            var results = await sqlExecutor.FindAsync<Goods>(new { id = id });
            return results.FirstOrDefault();
        }

        public async Task<IEnumerable<GoodsAttr>> GetGoodsAttributes(int goodsId)
        {
            return await sqlExecutor.FindAsync<GoodsAttr>(new { GoodsId = goodsId });
        }

        public async Task<IEnumerable<GoodsAttach>> GetGoodsAttachments(int goodsId)
        {
            return await sqlExecutor.FindAsync<GoodsAttach>(new { GoodsId = goodsId });
        }

        public async Task<int> Insert(Goods goods, IEnumerable<GoodsAttr> attrs, IEnumerable<GoodsAttach> downloads)
        {
            var trans = sqlExecutor.BeginTransaction();
            try
            {
                int id = await sqlExecutor.InsertAsync(goods, trans);
                foreach (var attr in attrs)
                {
                    attr.GoodsId = id;
                    await sqlExecutor.InsertAsync<GoodsAttr>(attr, trans);
                }
                foreach (var attach in downloads)
                {
                    attach.GoodsId = id;
                    await sqlExecutor.InsertAsync(attach, trans);
                }
                trans.Commit();
                return id;
            }
            catch (System.Exception ex)
            {
                trans.Rollback();
                return -999999;
            }
        }

        public async Task<int> Update(Goods goods, IEnumerable<GoodsAttr> attrs, IEnumerable<GoodsAttach> downloads)
        {
            var trans = sqlExecutor.BeginTransaction();
            try
            {
                int result = await sqlExecutor.ExecuteAsync("update goods set \"Name\"=@Name,\"Picture\"=@Picture,\"Info\"=@Info,\"Description\"=@Description,\"DisplayOrder\"=@DisplayOrder,\"Status\"=@Status where id=@id", new
                {
                    id = goods.id,
                    Name = goods.Name,
                    Picture = goods.Picture,
                    Info = goods.Info,
                    Description = goods.Description,
                    DisplayOrder = goods.DisplayOrder,
                    Status = goods.Status
                }, System.Data.CommandType.Text, trans);
                var items = await sqlExecutor.FindAsync<GoodsAttr>(new { GoodsId = goods.id }, trans);
                foreach (var item in items)
                {
                    await sqlExecutor.DeleteAsync(item);
                }
                foreach (var attr in attrs) attr.GoodsId = goods.id;
                await sqlExecutor.BulkInsertAsync<GoodsAttr>(attrs, trans);
                var rs = await sqlExecutor.FindAsync<GoodsAttach>(new { GoodsId = goods.id }, trans);
                foreach (var r in rs)
                {
                    await sqlExecutor.DeleteAsync(r);
                }
                foreach (var attach in downloads) attach.GoodsId = goods.id;
                await sqlExecutor.BulkInsertAsync<GoodsAttach>(downloads, trans);
                trans.Commit();
                return result;
            }
            catch (System.Exception ex)
            {
                trans.Rollback();
                return -999999;
            }
        }

        public async Task<int> Delete(int id)
        {
            var trans = sqlExecutor.BeginTransaction();
            try
            {
                var attrs = await sqlExecutor.FindAsync<GoodsAttr>(new { GoodsId = id }, trans);
                foreach (var attr in attrs)
                {
                    await sqlExecutor.DeleteAsync(attr, trans);
                }
                var attachments = await sqlExecutor.FindAsync<GoodsAttach>(new { GoodsId = id }, trans);
                foreach (var attach in attachments)
                {
                    await sqlExecutor.DeleteAsync(attach, trans);
                }
                var goodslist = await sqlExecutor.FindAsync<Goods>(new { id = id }, trans);
                foreach (var goods in goodslist)
                {
                    await sqlExecutor.DeleteAsync(goods, trans);
                }
                trans.Commit();
                return 0;
            }
            catch (System.Exception ex)
            {
                return -999999;
            }


        }
    }
}
