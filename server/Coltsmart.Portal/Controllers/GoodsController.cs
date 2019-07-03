using ColtSmart.Entity;
using ColtSmart.Entity.Entities;
using ColtSmart.Service;
using ColtSmart.Service.Service;
using Microsoft.AspNetCore.Mvc;
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

        [HttpDelete]
        [Route("api/goods/{id}")]
        public async Task<int> Delete(int id)
        {
            return await goodsService.Delete(id);
        }
    }
}
