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

namespace coltsmart.server.Controllers
{
    public class LoginController : ApiController
    {
        private IUserService userService = null;
        private readonly DbOptions dbOptions = null;
        private readonly IMemoryCache cache;

        public object EncryptionManager { get; private set; }

        public LoginController(IUserService userService, DbOptions dbOptions, IMemoryCache cache)
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
            Dictionary<string, string> keyPair = GetRSAKeyPair();
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
                    //IsDefaultPassword = false
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
        public async Task<IResult> Register([FromBody]TUser user)
        {
            if(user == null ||string.IsNullOrEmpty(user.RegEmall))
            {
                return new ErrorResult<string>("注册信息无效");
            }
            if(string.IsNullOrEmpty(user.NewPassword))
            {
                return new ErrorResult<string>("验证码不能为空");
            }
            string verifyCode = "";
            if(!cache.TryGetValue(user.RegEmall,out verifyCode)||string.IsNullOrEmpty(verifyCode))
            {
                return new ErrorResult<string>("验证码无效，请重新获取新的验证码");
            }
            if(0 != user.NewPassword.Trim().CompareTo(verifyCode))
            {
                return new ErrorResult<string>("验证码不正确，请重新填写");
            }

            Dictionary<string, string> keyPair = GetRSAKeyPair();
            //解密登录密码
            user.Password = EncryptionProvider.DecryptRSA(user.Password, keyPair["PRIVATE"]);
            return await userService.Register(user);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/api/login/sendverifycode")]
        public IResult SendVerifyCode(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new ErrorResult<string>("邮箱不能为空");
            }
 
            //生成6为随机验证码
            string verifyCode = CreateVerifyCode(6);

            //TODO:维护发送验证码的邮箱信息
            string defaultEmail = "jim-lung@163.com";
            string authCode = "1qaz2wsx";
            string defaultHost = "smtp.163.com";
            int defaultPort = 25;
            string defaultSubject = "【鸣驹智能】邮箱注册验证码";
            string defaultBody = "欢迎您注册鸣驹智能，您的注册码为{code}";

            MailMessage message = new MailMessage(defaultEmail, email);
            message.Subject = defaultSubject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = defaultBody.Replace("{code}", verifyCode);

            //发送邮件
            SmtpClient client = new SmtpClient(defaultHost,defaultPort);           
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(defaultEmail, authCode);
            client.Send(message);

            //缓存验证码到内存中，以备注册时进行校验
            cache.Set<string>(email, verifyCode);
            return new BaseResult<string>("发送成功，请注意查收");
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
        public async Task<IResult> ModifyPassword([FromBody]TUser user)
        {
            Dictionary<string, string> keyPair = GetRSAKeyPair();
            user.Password = EncryptionProvider.DecryptRSA(user.Password, keyPair["PRIVATE"]);
            user.NewPassword = EncryptionProvider.DecryptRSA(user.NewPassword, keyPair["PRIVATE"]);

            return await userService.ModifyPassword(user);
        }
        
        [HttpPost]
        [Route("api/Login/resetpassword")]
        public async Task<IResult> ResetPassword([FromBody]TUser user)
        {
            if(user == null ||string.IsNullOrEmpty(user.RegEmall))
            {
                return new ErrorResult<string>("注册信息无效");
            }
            if (string.IsNullOrEmpty(user.UserName))
            {
                return new ErrorResult<string>("用户名不能为空");
            }
            if (string.IsNullOrEmpty(user.NewPassword))
            {
                return new ErrorResult<string>("验证码不能为空");
            }
            string verifyCode = "";
            if(!cache.TryGetValue(user.RegEmall,out verifyCode)||string.IsNullOrEmpty(verifyCode))
            {
                return new ErrorResult<string>("验证码无效，请重新获取新的验证码");
            }
            if(0 != user.NewPassword.Trim().CompareTo(verifyCode))
            {
                return new ErrorResult<string>("验证码不正确，请重新填写");
            }
            return await userService.ResetPassword(user);
        }

    }
}