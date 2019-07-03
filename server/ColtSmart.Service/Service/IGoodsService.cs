using ColtSmart.Entity.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ColtSmart.Service.Service
{
    public interface IGoodsService
    {
        Task<PagedResult<Goods>> GetGoods(int page,int pageSize,string name);

        Task<Goods> GetGoods(int id);

        Task<IEnumerable<GoodsAttr>> GetGoodsAttributes(int goodsId);

        Task<IEnumerable<GoodsAttach>> GetGoodsAttachments(int goodsId);

        Task<int> Insert(Goods goods);

        Task<int> Update(Goods goods);

        Task<int> Delete(int id);
    }
}
