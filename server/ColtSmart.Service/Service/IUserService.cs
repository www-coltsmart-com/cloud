using ColtSmart.Entity.Entities;
using System.Threading.Tasks;

namespace ColtSmart.Service.Service
{
    public interface IUserService
    {
        bool VerifyUser(string username, string password);

        Task<bool> ModifyPassword(TUser user);

        Task<bool> ResetPassword(TUser user);

        Task<TUser> GetUser(int id);

        TUser GetUser(string userno);

        Task<PagedResult<TUser>> GetUsers(int page, int pageSize, string userName);

        Task<bool> Delete(int id);

        Task<bool> Create(TUser user);

        Task<bool> Update(TUser user);

        Task<bool> Register(TUser user);

        Task<int> GetUserCount();
    }
}
