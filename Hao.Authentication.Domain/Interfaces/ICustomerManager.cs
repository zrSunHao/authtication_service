using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface ICustomerManager
    {
        public Task<ResponsePagingResult<CtmM>> GetList(PagingParameter<CtmFilter> param);

        public Task<ResponseResult<bool>> AddRemark(string ctmId, string remark);

        public Task<ResponseResult<bool>> ResetPsd(string ctmId, string psd);

        public Task<ResponseResult<CtmM>> GetById(string ctmId);

        public Task<ResponseResult<PeopleM>> GetPeople(string ctmId);

        public Task<ResponseResult<bool>> UpdatePeople(PeopleM model);


        public Task<ResponsePagingResult<CtmRoleM>> GetRoleList(PagingParameter<CtmRoleFilter> param);

        public Task<ResponseResult<bool>> AddRole(CtmRoleUpdateM model);

        public Task<ResponseResult<bool>> UpdateRole(CtmRoleUpdateM model);

        public Task<ResponseResult<bool>> DeleteRole(string ctmId, string roleId);


        public Task<ResponsePagingResult<CttM>> GetCttList(PagingParameter<CtmCttFilter> param);

        public Task<ResponseResult<bool>> AddCtt(CtmCttAddM model);

        public Task<ResponsePagingResult<CtmLogM>> GetLogList(PagingParameter<CtmLogFilter> param);

    }
}
