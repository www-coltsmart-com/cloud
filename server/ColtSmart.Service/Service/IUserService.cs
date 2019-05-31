using ColtSmart.Entity.Entities;
using System.Threading.Tasks;

namespace ColtSmart.Service.Service
{
    public interface IUserService
    {
        bool VerifyUser(string username, string password);

        IResult ModifyPassword(TUser user);

        TUser GetUser(string userno);

        Task<PagedResult<TUser>> GetUsers(int page, int pageSize, string userName);

        Task<int> Delete(int id);

        Task<IResult> Create(TUser user);

        Task<IResult> Register(TUser user);
    }
}
