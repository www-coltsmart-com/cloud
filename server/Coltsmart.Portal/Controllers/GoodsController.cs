using ColtSmart.Entity;
using ColtSmart.Entity.Entities;
using ColtSmart.Service;
using ColtSmart.Service.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Coltsmart.Portal.Controllers
{
    public class GoodsController : ApiController
    {
        private IGoodsService goodsService = null;

        public GoodsController(IGoodsService goodsService)
        {
            this.goodsService = goodsService;
        }

        [HttpGet]
        [Route("api/goods")]
        public async Task<PagedResult<Goods>> Get(int page, int size, string name)
        {
            return await goodsService.GetGoods(page, size, name);
        }

        [HttpGet]
        [Route("api/goods/{id}")]
        public async Task<Goods> Get(int id)
        {
            return await goodsService.GetGoods(id);
        }

        [HttpGet]
        [Route("api/goods/{id}/attr")]
        public async Task<IEnumerable<GoodsAttr>> GetAttr(int goodsId)
        {
            return await goodsService.GetGoodsAttributes(goodsId);
        }

        [HttpGet]
        [Route("api/goods/{id}/attach")]
        public async Task<IEnumerable<GoodsAttach>> GetAttach(int goodsId)
        {
            return await goodsService.GetGoodsAttachments(goodsId);
        }

        [HttpDelete]
        [Route("api/goods/{id}")]
        public async Task<int> Delete(int id)
        {
            return await goodsService.Delete(id);
        }
    }
}
