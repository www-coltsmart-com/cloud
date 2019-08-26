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

        public async Task<bool> ModifyPassword(TUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserNo) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.NewPassword))
                return false;
            var users = await sqlExecutor.FindAsync<TUser>(new { UserNo = user.UserNo });
            var retUser = users.FirstOrDefault();
            if (retUser == null) return false;
            string password = EncryptHelper.Instance.PassEncryption(user.UserNo, user.Password.Trim());
            if (retUser.Password != password)
                return false;
            retUser.Password = retUser.NewPassword = EncryptHelper.Instance.PassEncryption(user.UserNo, user.NewPassword);
            var result = await sqlExecutor.UpdateAsync<TUser>(retUser);
            return result > 0;
        }

        public async Task<bool> ResetPassword(TUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.RegEmall)) return false;
            var users = await sqlExecutor.FindAsync<TUser>(new { UserNo = user.UserName, RegEmall = user.RegEmall });
            var retUser = users.FirstOrDefault();
            if (retUser == null) return false;
            retUser.Password = retUser.NewPassword = EncryptHelper.Instance.PassEncryption(retUser.UserNo, "654321");
            var result = await sqlExecutor.UpdateAsync<TUser>(retUser);
            return result > 0;
        }

        public bool VerifyUser(string usercode, string password)
        {
            var user = sqlExecutor.Find<TUser>(new { UserNo = usercode }).FirstOrDefault();
            if (user == null) return false;
            var encrptPassword = EncryptHelper.Instance.PassEncryption(user.UserNo, password);
            string str = EncryptHelper.Instance.PassEncryption(user.UserNo, "654321");
            return user.Password == encrptPassword;
        }

        public TUser GetUser(string userno)
        {
            return sqlExecutor.Find<TUser>(new { UserNo = userno }).FirstOrDefault();
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

        public async Task<bool> Delete(int id)
        {
            var users = await sqlExecutor.FindAsync<TUser>(new { Id = id });
            if (users == null || !users.Any()) return false;
            var user = users.First();
            return await sqlExecutor.DeleteAsync<TUser>(user) > 0;
        }

        public async Task<bool> Create(TUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserNo)) return false;
            var users = await sqlExecutor.FindAsync<TUser>(new { UserNo = user.UserNo });
            if (users == null || !users.Any()) return false;
            user.UserNo = user.UserNo.Trim();
            user.UserType = EUserType.Admin;//后台手工添加用户为管理员，通过注册用户则为普通用户
            user.Password = ColtSmart.Encrypt.EncryptHelper.Instance.PassEncryption(user.UserNo, "654321");
            user.RegDate = DateTime.Now;
            return await sqlExecutor.InsertAsync<TUser>(user) > 0;
        }

        public async Task<bool> Register(TUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserNo) || string.IsNullOrEmpty(user.Password)) return false;
            var results = await sqlExecutor.FindAsync<TUser>(new { UserNo = user.UserNo });
            var retUser = results.FirstOrDefault();
            if (retUser != null) return false;
            user.UserName = string.IsNullOrEmpty(user.UserName) ? "注册用户" : user.UserName.Trim();
            //密码加密后保存
            user.Password = EncryptHelper.Instance.PassEncryption(user.UserNo, user.Password.Trim());
            user.RegDate = DateTime.Now;
            user.UserType = EUserType.Custom;//注册入口均为普通用户，管理员为后台手工分配
            int result = await sqlExecutor.InsertAsync<TUser>(user);
            return result > 0;
        }

        public async Task<int> GetUserCount()
        {
            return await sqlExecutor.ExecuteScalarAsync<int>("SELECT * FROM tuser");
        }
    }
}
