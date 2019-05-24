using ColtSmart.Core.Encrypt;
using ColtSmart.Data;
using ColtSmart.Entity.Entities;
using ColtSmart.JWT;
using ColtSmart.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace coltsmart.server.Controllers
{
    public class LoginController: ApiController
    {
        private IUserService userService = null;
        private readonly DbOptions dbOptions = null;
        private readonly IMemoryCache cache;

        public object EncryptionManager { get; private set; }

        public LoginController(IUserService userService,DbOptions dbOptions, IMemoryCache cache)
        {
            this.userService = userService;
            this.dbOptions = dbOptions;
            this.cache = cache;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Login")]
        public ActionResult Login([FromBody]TUser user)
        {
            Dictionary<string, string> keyPair = null;
            if (cache.Get("RSAKEYPAIR") == null)
            {
                keyPair = EncryptionProvider.CreateRsaKeyPair();
                cache.Set("RSAKEYPAIR", keyPair, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
            }
            else
            {
                keyPair = cache.Get("RSAKEYPAIR") as Dictionary<string, string>;
            }

            var password = EncryptionProvider.DecryptRSA(user.Password, keyPair["PRIVATE"]);

            if (userService.VerifyUser(user.UserName, password))
            {
                return Ok<string>(JwtManager.GenerateToken(user.UserName));
            }

            return StatusCode(System.Net.HttpStatusCode.Unauthorized);
        }

        [HttpGet]
        [Route("api/Login/userinfo")]
        public dynamic UserInfo(string userName)
        {
           var user= userService.GetUser(userName);

            if (user != null)
            {
                return new
                {
                    Id = user.Id,
                    IsAdmin = user.UserType== EUserType.Admin,
                    TotalDevice = 1000,
                    TotalUser = 140,
                    OnlineDevice = 340,
                    EMail = user.RegEmall,
                    MobilePhone =user.MobilePhone,
                    Company = user.Company,
                    RegDate = user.RegDate
                };
            }
            else
            {
                return new
                {
                    Id = 0,
                    IsAdmin = false,
                    TotalDevice = 0,
                    TotalUser = 0,
                    OnlineDevice = 0,
                    EMail = "",
                    MobilePhone = "",
                    Company = "",
                    RegDate = ""
                };
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Login/getpublickey")]
        public string GetPublicKey()
        {
            Dictionary<string, string> keyPair = null;
            if (cache.Get("RSAKEYPAIR") == null)
            {
                keyPair = EncryptionProvider.CreateRsaKeyPair();
                cache.Set("RSAKEYPAIR", keyPair, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
            }
            else
            {
                keyPair = cache.Get("RSAKEYPAIR") as Dictionary<string, string>;
            }

            return keyPair["PEMPUBLIC"];
        }
    }
}