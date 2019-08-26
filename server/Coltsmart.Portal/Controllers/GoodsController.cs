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
using System;

namespace Coltsmart.Portal.Controllers
{
    public class GoodsController : ApiController
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private IGoodsService goodsService = null;

        public GoodsController(IHostingEnvironment hostingEnvironment, IGoodsService goodsService)
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
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest();
            var goods = await goodsService.GetGoods(id);
            if (goods == null) return BadRequest();

            var attrs = await goodsService.GetGoodsAttributes(id);
            var downloads = await goodsService.GetGoodsAttachments(id);
            var result = goods.ToModel();
            result.attrs = attrs.ToModel();
            result.downloads = downloads.ToModel();
            return Ok(result);
        }

        //删除
        [HttpDelete]
        [Route("api/goods/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await goodsService.Delete(id);
            return Ok();
        }

        //上传图片
        [HttpPost]
        [Route("api/uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            //有效判断
            if (file == null || file.Length <= 0) return BadRequest();
            string fileName = file.FileName;
            string fileExt = Path.GetExtension(file.FileName);
            long fileSize = file.Length;

            try
            {
                string rootPath = hostingEnvironment.ContentRootPath;//根目录
                string uploadPath = Path.Combine("upload", "temp", DateTime.Today.ToString("yyyyMMdd"));//上传目录
                string newFileName = Guid.NewGuid().ToString() + fileExt;//新文件名称，唯一标识

                //上传目录
                string path = Path.Combine(rootPath, uploadPath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //删除过期文件及目录
                string oldPath = Path.Combine(rootPath, "upload", "temp", DateTime.Today.AddDays(-15).ToString("yyyyMMdd"));
                if (Directory.Exists(oldPath))
                {
                    Directory.Delete(oldPath, true);
                }
                //上传文件
                string filePath = Path.Combine(path, newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Ok(new
                {
                    name = fileName,
                    ext = fileExt,
                    size = fileSize,
                    path = Path.Combine(uploadPath, newFileName)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/goods")]
        public async Task<IActionResult> Save([FromBody]GoodsModel goods)
        {
            if (goods == null) return BadRequest();

            //处理商品主图
            if (!string.IsNullOrEmpty(goods.picture))
            {
                goods.picture = ParseUploadFile(goods.picture);
            }
            //处理附件
            if (goods.downloads != null && goods.downloads.Any())
            {
                foreach (var item in goods.downloads)
                {
                    item.url = ParseUploadFile(item.url);
                }
            }

            //如果有id，则为修改，否则就是新增。
            if (goods.id > 0)
            {
                await goodsService.Update(goods.ToEntity(), goods.attrs.ToEntity(), goods.downloads.ToEntity());
            }
            else
            {
                await goodsService.Insert(goods.ToEntity(), goods.attrs.ToEntity(), goods.downloads.ToEntity());
            }
            return Ok();
        }
        // 处理上传文件：从临时上传目录(/upload/temp/yyyymmdd)中删除，移动到正式目录(/upload)下。
        private string ParseUploadFile(string uriPath)
        {
            try
            {
                Uri url = new Uri(uriPath);
                string filePath = url.LocalPath;
                string fileName = Path.GetFileName(filePath);

                string sourceFilePath = hostingEnvironment.ContentRootPath + filePath;
                string destFilePath = Path.Combine(hostingEnvironment.ContentRootPath, "upload", fileName);
                if (File.Exists(sourceFilePath))
                {
                    File.Move(sourceFilePath, destFilePath);
                    return new UriBuilder(url.Scheme,url.Host,url.Port,Path.Combine("upload", fileName)).ToString();
                }
                else
                {
                    return sourceFilePath;
                }
            }
            catch
            {
                return uriPath;
            }
        }
    }
}
