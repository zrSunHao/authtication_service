using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface IConstraintManager
    {
        public Task<ResponseResult<bool>> Cancel(string id);

        public Task<ResponsePagingResult<CttM>> GetList(PagingParameter<CttFilter> param);
    }
}
