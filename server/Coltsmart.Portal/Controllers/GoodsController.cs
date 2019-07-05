using ColtSmart.Entity;
using ColtSmart.Entity.Entities;
using ColtSmart.Service;
using ColtSmart.Service.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using System.IO;
using Coltsmart.Portal.Models;

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

        [HttpPost]
        [Route("api/uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return Ok(new
            {
                size = file.Length,
                filePath
            });
        }


        [HttpPost]
        [Route("api/uploadfiles")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePath = Path.GetTempFileName();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new
            {
                count = files.Count,
                size,
                filePath
            });
        }

        /* public async Task<int> Insert(GoodsModel goods)
        {
            throw new System.Exception();
        } */
    }
}
