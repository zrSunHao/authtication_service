using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface ICustomerManager
    {
        public Task<ResponsePagingResult<CtmM>> GetList(PagingParameter<CtmFilter> param);

        public Task<ResponseResult<bool>> AddRemark(string remark);

        public Task<ResponseResult<bool>> ResetPsd(string psd);

        public Task<ResponseResult<PeopleM>> GetPeople(string ctmId);

        public Task<ResponseResult<bool>> UpdatePeople(PeopleM model);


        public Task<ResponsePagingResult<CtmRoleFilter>> GetRoleList(PagingParameter<CtmRoleFilter> param);

        public Task<ResponseResult<bool>> AddRole(CtmRoleAddM model);

        public Task<ResponseResult<bool>> UpdateRole(CtmRoleAddM model);


        public Task<ResponsePagingResult<CtmRoleView>> GetCttList(PagingParameter<CtmCttFilter> param);

        public Task<ResponseResult<bool>> AddCtt(CtmCttAddM modelm);

        public Task<ResponsePagingResult<CtmLogM>> GetLogList(PagingParameter<CtmLogFilter> param);

    }
}
