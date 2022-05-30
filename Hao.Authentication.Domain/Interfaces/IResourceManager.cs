using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface IResourceManager
    {
        public Task<ResponseResult<bool>> Save(ResourceM model);

        public Task<ResponseResult<ResourceM>> GetByCode(string code);

        public string GetNewCode();
    }
}
