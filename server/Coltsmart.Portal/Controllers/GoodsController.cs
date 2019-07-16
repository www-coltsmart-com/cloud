using ColtSmart.Entity;
using ColtSmart.Entity.Entities;
using ColtSmart.Service;
using ColtSmart.Service.Service;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment hostingEnvironment;
        private IGoodsService goodsService = null;

        public GoodsController(IHostingEnvironment hostingEnvironment,IGoodsService goodsService)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.goodsService = goodsService;
        }

        //分页获取列表
        [HttpGet]
        [Route("api/goods")]
        public async Task<PagedResult<Goods>> Get(int page, int size, string name)
        {
            return await goodsService.GetGoods(page, size, name);
        }

        //获取单条记录
        [HttpGet]
        [Route("api/goods/{id}")]
        public async Task<IResult> Get(int id)
        {
            try
            {
                if (id <= 0) throw new System.Exception("参数无效");
                var goods = await goodsService.GetGoods(id);
                if (goods == null) throw new System.Exception("产品信息不存在");
                var attrs = await goodsService.GetGoodsAttributes(id);
                var downloads = await goodsService.GetGoodsAttachments(id);
                var result = goods.ToModel();
                result.attrs = attrs.ToModel();
                result.downloads = downloads.ToModel();
                return new BaseResult<GoodsModel>(result);
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }

        //删除
        [HttpDelete]
        [Route("api/goods/{id}")]
        public async Task<int> Delete(int id)
        {
            return await goodsService.Delete(id);
        }

        //上传图片
        [HttpPost]
        [Route("api/uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string path = hostingEnvironment.
            




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
        [Route("api/goods")]
        public async Task<IResult> Save([FromBody]GoodsModel goods)
        {
            if (goods == null)
                return new ErrorResult<string>("信息无效，请重新提交");
            int result = 0;
            if (goods.id > 0)
            {
                result = await goodsService.Update(goods.ToEntity(), goods.attrs.ToEntity(), goods.downloads.ToEntity());
            }
            else
            {
                result = await goodsService.Insert(goods.ToEntity(), goods.attrs.ToEntity(), goods.downloads.ToEntity());
            }
            return new BaseResult<int>(result);
        }
    }
}
