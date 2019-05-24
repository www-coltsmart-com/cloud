using ColtSmart.Data;
using ColtSmart.Entity.Entities;
using ColtSmart.Service.Service;
using System.Linq;
using System;
using ColtSmart.Encrypt;

namespace ColtSmart.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly ISqlExecutor sqlExecutor;

        public UserService(ISqlExecutor sqlExecutor)
        {
            this.sqlExecutor = sqlExecutor;
        }

        public IResult ModifyPassword(TUser user)
        {
            var retUser = sqlExecutor.Find<TUser>(new { UserName = user.UserName }).FirstOrDefault();

            if (retUser == null)
                return new ErrorResult<string>("该用户不存在");

            if (retUser.Password != user.Password)
                return new ErrorResult<string>("原密码错误，修改失败！");

            retUser.Password = user.NewPassword;
            var result = sqlExecutor.Update<TUser>(retUser);

            return new BaseResult<int>(result);
        }

        public bool VerifyUser(string usercode, string password)
        {
            var user = sqlExecutor.Find<TUser>(new { UserNo = usercode }).FirstOrDefault();
            var encrptPassword= EncryptHelper.Instance.PassEncryption(usercode, password);

            var ret= user != null && user.Password == encrptPassword;
            return ret;
        }

        public TUser GetUser(string userno)
        {
            var user = sqlExecutor.Find<TUser>(new { UserNo = userno }).FirstOrDefault();
            return user;
        }
    }
}
