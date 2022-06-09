using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface IConstraintManager
    {
        public Task<ResponseResult<bool>> Cancel(string id);

        public Task<ResponsePagingResult<CttM>> GetList(PagingParameter<CttFilter> param);

        /// <summary>
        /// 添加约束
        /// </summary>
        /// <returns></returns>
        public Task<ResponseResult<bool>> Add(CttAddM ctt,bool directlySave = false);

        public void AutoUpdateCache(object? o);
    }
}
