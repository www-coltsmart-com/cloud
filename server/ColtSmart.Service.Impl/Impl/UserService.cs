using ColtSmart.Data;
using ColtSmart.Entity.Entities;
using ColtSmart.Service.Service;
using System.Linq;
using System;
using ColtSmart.Encrypt;
using System.Text;

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
            var encrptPassword = EncryptHelper.Instance.PassEncryption(usercode, password);

            var ret = user != null && user.Password == encrptPassword;
            return ret;
        }

        public TUser GetUser(string userno)
        {
            var user = sqlExecutor.Find<TUser>(new { UserNo = userno }).FirstOrDefault();
            return user;
        }

        public PagedResult<TUser> GetUsers(int page, int pageSize, string userName)
        {
            StringBuilder sqlBuilder = new StringBuilder("SELECT * FROM tuser");
            object param = null;
            if (!string.IsNullOrEmpty(userName))
            {
                sqlBuilder.Append(" WHERE \"UserName\" like @UserName");
                param = new { UserName = string.Format("{0}%", userName.Trim()) };
            }
            return sqlExecutor.QueryPage<TUser>(sqlBuilder.ToString(), page, pageSize, param).ToPagedResult();
        }

        public int DeleteUser(int id)
        {
            TUser user = sqlExecutor.Find<TUser>(new { Id = id }).FirstOrDefault();
            if (user != null)
            {
                return sqlExecutor.Delete<TUser>(user);
            }
            return 0;
        }

        public IResult RegisterUser(TUser user)
        {
            var retUser = sqlExecutor.Find<TUser>(new { UserNo = user.UserNo }).FirstOrDefault();
            if (retUser != null)
                return new ErrorResult<string>("用户名已存在，请重新输入一个新的用户名");
            if (string.IsNullOrEmpty(user.Password))
                return new ErrorResult<string>("密码不能为空");
            user.UserName = string.IsNullOrEmpty(user.UserName) ? "注册用户" : user.UserName.Trim();
            //密码加密后保存
            user.Password = EncryptHelper.Instance.PassEncryption(user.UserNo, user.Password.Trim());
            user.RegDate = DateTime.Now;
            user.UserType = EUserType.Custom;//注册入口均为普通用户，管理员为后台手工分配
            int result = sqlExecutor.Insert<TUser>(user);
            return new BaseResult<int>(result);
        }
    }
}
