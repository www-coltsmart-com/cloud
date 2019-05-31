using ColtSmart.Data;
using ColtSmart.Entity.Entities;
using ColtSmart.Service.Service;
using System.Linq;
using System;
using ColtSmart.Encrypt;
using System.Text;
using System.Threading.Tasks;

namespace ColtSmart.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly ISqlExecutor sqlExecutor;

        public UserService(DbOptions dbOptions)
        {
            this.sqlExecutor = new SqlExecutor(dbOptions);
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

        public async Task<PagedResult<TUser>> GetUsers(int page, int pageSize, string userName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(userName))
            {
                if (sqlBuilder.Length > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append(" \"UserName\" LIKE @UserName ");
            }
            object param = new
            {
                UserName = string.IsNullOrEmpty(userName) ? "%" : string.Format("{0}%", userName.Trim())
            };
            if (sqlBuilder.Length > 0) sqlBuilder.Insert(0, " WHERE ");
            sqlBuilder.Insert(0, "SELECT * FROM tuser");
            var result = await sqlExecutor.QueryPageAsync<TUser>(sqlBuilder.ToString(), page, pageSize, param);
            return result.ToPagedResult();
        }

        public async Task<int> Delete(int id)
        {
            var result = await sqlExecutor.FindAsync<TUser>(new { Id = id });
            var user = result.FirstOrDefault();
            if (user != null)
            {
                return await sqlExecutor.DeleteAsync<TUser>(user);
            }
            return 0;
        }

        public async Task<IResult> Register(TUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserNo) || string.IsNullOrEmpty(user.Password))
            {
                return new ErrorResult<string>("用户名或密码不能为空");
            }
            var results = await sqlExecutor.FindAsync<TUser>(new { UserNo = user.UserNo });
            var retUser = results.FirstOrDefault();
            if (retUser != null)
            {
                return new ErrorResult<string>("用户名已存在，请重新输入一个新的用户名");
            }
            user.UserName = string.IsNullOrEmpty(user.UserName) ? "注册用户" : user.UserName.Trim();
            //密码加密后保存
            user.Password = EncryptHelper.Instance.PassEncryption(user.UserNo, user.Password.Trim());
            user.RegDate = DateTime.Now;
            user.UserType = EUserType.Custom;//注册入口均为普通用户，管理员为后台手工分配
            int result = await sqlExecutor.InsertAsync<TUser>(user);
            return new BaseResult<int>(result);
        }
    }
}
