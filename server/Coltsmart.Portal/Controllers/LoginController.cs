using ColtSmart.Core.Encrypt;
using ColtSmart.Data;
using ColtSmart.Encrypt;
using ColtSmart.Entity.Entities;
using ColtSmart.JWT;
using ColtSmart.Service;
using ColtSmart.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Text;
using Coltsmart.Portal.Models;

namespace coltsmart.server.Controllers
{
    public class LoginController : ApiController
    {
        private IUserService userService = null;
        private IDeviceService deviceService = null;
        private readonly DbOptions dbOptions = null;
        private readonly IMemoryCache cache;

        public object EncryptionManager { get; private set; }

        public LoginController(IUserService userService, IDeviceService deviceService, DbOptions dbOptions, IMemoryCache cache)
        {
            this.userService = userService;
            this.deviceService = deviceService;
            this.dbOptions = dbOptions;
            this.cache = cache;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Login")]
        public ActionResult Login([FromBody]TUser user)
        {
            Dictionary<string, string> keyPair = GetRSAKeyPair();
            var password = EncryptionProvider.DecryptRSA(user.Password, keyPair["PRIVATE"]);
            if (userService.VerifyUser(user.UserName, password))
            {
                return Ok<string>(JwtManager.GenerateToken(user.UserName));
            }
            else
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }
        }

        [HttpGet]
        [Route("api/Login/userinfo")]
        public dynamic UserInfo(string userName)
        {
            var user = userService.GetUser(userName);
            if (user != null)
            {
                return new
                {
                    Id = user.id,
                    IsAdmin = user.UserType == EUserType.Admin,
                    TotalDevice = 1000,
                    TotalUser = 140,
                    OnlineDevice = 340,
                    EMail = user.RegEmall,
                    MobilePhone = user.MobilePhone,
                    Company = user.Company,
                    RegDate = user.RegDate,
                    IsDefaultPassword = user.Password.Equals(EncryptHelper.Instance.PassEncryption(user.UserNo, "654321"))
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
                    RegDate = "",
                    IsDefaultPassword = false
                };
            }
        }

        [HttpGet]
        [Route("api/Login/getstatsinfo")]
        public async Task<IActionResult> GetStatsInfo(string userNo)
        {
            if (string.IsNullOrEmpty(userNo))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            string key = string.Format("{0}_STATS_INFO", userNo);
            if (cache.Get(key) != null)
            {
                return Ok<StatsInfo>(cache.Get(key) as StatsInfo);
            }
            var user = userService.GetUser(userNo);
            if (user == null) return StatusCode(HttpStatusCode.Unauthorized);
            bool isAdmin = user.UserType == EUserType.Admin;//判断是否为管理员身份，用于显示不同权限的统计数据
            int totalDeviceCount = await deviceService.GetDeviceCount((isAdmin ? "" : user.UserNo));
            int onlineDeviceCount = await deviceService.GetDeviceCount((isAdmin ? "" : user.UserNo), true);
            int totalUserCount = await userService.GetUserCount();
            var value = new StatsInfo()
            {
                TotalDeviceCount = totalDeviceCount,
                TotalDeviceDisplay = true,
                OnlineDeviceCount = onlineDeviceCount,
                OnlineDeviceDisplay = true,
                TotalUserCount = totalUserCount,
                TotalUserDisplay = isAdmin
            };
            cache.Set(key, value, new DateTimeOffset(DateTime.Now.AddMinutes(5)));
            return Ok(value);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Login/getpublickey")]
        public string GetPublicKey()
        {
            Dictionary<string, string> keyPair = GetRSAKeyPair();
            return keyPair["PEMPUBLIC"];
        }

        private Dictionary<string, string> GetRSAKeyPair()
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
            return keyPair;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Login/savereg")]
        public async Task<IActionResult> Register([FromBody]TUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.RegEmall))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            string verifyCode = "";
            if (string.IsNullOrEmpty(user.NewPassword) || !cache.TryGetValue(user.RegEmall, out verifyCode) || string.IsNullOrEmpty(verifyCode) || 0 != user.NewPassword.Trim().CompareTo(verifyCode))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            Dictionary<string, string> keyPair = GetRSAKeyPair();
            //解密登录密码
            user.Password = EncryptionProvider.DecryptRSA(user.Password, keyPair["PRIVATE"]);
            var result = await userService.Register(user);
            return result ? Ok() : StatusCode(HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/api/login/sendverifycode")]
        public IActionResult SendVerifyCode(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            //生成6为随机验证码
            string verifyCode = CreateVerifyCode(6);

            try
            {
                //TODO:在数据库表中维护系统邮箱账号等信息  dxj
                string defaultEmail = "jim-lung@163.com";
                string authCode = "1qaz2wsx";
                string defaultHost = "smtp.163.com";
                int defaultPort = 25;
                string defaultSubject = "【鸣驹智能】邮箱注册验证码";
                string defaultBody = "你好，欢迎你注册鸣驹智能平台，您的注册码为{code}，请及时进行验证。";

                MailMessage message = new MailMessage(defaultEmail, email);
                message.Subject = defaultSubject;
                message.Body = defaultBody.Replace("{code}", verifyCode);

                //发送邮件
                SmtpClient client = new SmtpClient(defaultHost, defaultPort);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(defaultEmail, authCode);
                client.Send(message);

                //缓存验证码到内存中，以备注册时进行校验
                cache.Set<string>(email, verifyCode);
                return Ok();
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        //生成6位数字和大写字母的验证码
        private string CreateVerifyCode(int length)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < length; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        [HttpPost]
        [Route("/api/login/modifypassword")]
        public async Task<IActionResult> ModifyPassword([FromBody]TUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.NewPassword))
                return StatusCode(HttpStatusCode.BadRequest);

            Dictionary<string, string> keyPair = GetRSAKeyPair();
            user.Password = EncryptionProvider.DecryptRSA(user.Password, keyPair["PRIVATE"]);
            user.NewPassword = EncryptionProvider.DecryptRSA(user.NewPassword, keyPair["PRIVATE"]);

            var result = await userService.ModifyPassword(user);
            return result ? Ok() : StatusCode(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        [Route("api/Login/resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody]TUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.RegEmall) || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.NewPassword))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            string verifyCode = "";
            if (!cache.TryGetValue(user.RegEmall, out verifyCode) || string.IsNullOrEmpty(verifyCode) || 0 != user.NewPassword.Trim().CompareTo(verifyCode))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            bool result = await userService.ResetPassword(user);
            return result ? Ok() : StatusCode(HttpStatusCode.BadRequest);
        }

    }
}