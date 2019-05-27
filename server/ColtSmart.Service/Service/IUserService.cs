using ColtSmart.Entity.Entities;

namespace ColtSmart.Service.Service
{
    public interface IUserService
    {
        bool VerifyUser(string username, string password);

        IResult ModifyPassword(TUser user);

        TUser GetUser(string userno);

        PagedResult<TUser> GetUsers(int page, int pageSize, string userName);

        int DeleteUser(int id);
    }
}
