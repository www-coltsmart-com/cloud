using ColtSmart.Entity.Entities;
using ColtSmart.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;

namespace Coltsmart.Portal.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("api/users")]
        public PagedResult<TUser> Get(int page, int size, string username)
        {
            var pr = new PagedResult<TUser>
            {
                CurrentPage = page,
                PageSize = size,
                TotalCount = Users.Count
            };

            pr.Result = Users.Skip((page - 1) * size).Take(size);

            return pr;
        }

        [HttpDelete]
        [Route("api/users/{id}")]
        public int Delete(int id)
        {
            return 0;
        }

        [HttpPost]
        public IResult Post([FromBody]TUser value)
        {
            value.RegDate = DateTime.Now;
            value.Password = ColtSmart.Encrypt.EncryptHelper.Instance.PassEncryption(value.UserNo, "654321");

            return null;
        }

        public static List<TUser> Users = new List<TUser>
        {
            new TUser
            {
                Id=100,
                DevCount =200,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@163.com",
                UserName ="刘小东",
                UserNo="liuxiaodong"
            },
            new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },
            new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            },new TUser
            {
                Id=101,
                DevCount =201,
                MobilePhone ="15865289221",
                RegDate =System.DateTime.Now,
                RegEmall ="328740754@qq.com",
                UserName ="马威",
                UserNo="mawei"
            }
        };

        public string EncryptHelper { get; private set; }
    }
}
