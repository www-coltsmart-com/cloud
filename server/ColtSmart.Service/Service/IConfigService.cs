using ColtSmart.Entity;
using ColtSmart.Entity.Entities;
using System.Threading.Tasks;

namespace ColtSmart.Service.Service
{
    public interface IConfigService
    {
        Task<Config> GetConfig(string key);

        Task<PagedResult<Config>> GetConfigs(int page, int pageSize, string name);
    }
}
