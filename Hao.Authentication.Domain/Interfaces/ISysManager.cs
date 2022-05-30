using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface ISysManager
    {
        public Task<ResponseResult<bool>> Add(SysM model);

        public Task<ResponseResult<bool>> Update(SysM model);

        public Task<ResponsePagingResult<SysM>> GetList(PagingParameter<SysFilter> param);

        public Task<ResponseResult<bool>> Delete(string id);



        public Task<ResponsePagingResult<SysProgramM>> GetOwnedPgmList(PagingParameter<SysOwnedPgmFilter> param);

        public Task<ResponsePagingResult<SysProgramM>> GetNotOwnedPgmList(PagingParameter<SysNotOwnedPgmFilter> param);

        public Task<ResponseResult<bool>> AddPgm(string sysId, string pgmId);

        public Task<ResponseResult<bool>> DeletePgm(string sysId, string pgmId);


        public Task<ResponsePagingResult<SysCtmM>> GetCtms(PagingParameter<SysCtmFilter> param);



        public Task<ResponseResult<bool>> AddRole(SysRoleM model);

        public Task<ResponseResult<bool>> UpdateRole(SysRoleM model);

        public Task<ResponsePagingResult<SysRoleM>> GetRoleList(PagingParameter<SysRoleFilter> param);

        public Task<ResponseResult<bool>> DeleteRole(string id);

        public Task<ResponsePagingResult<SysRolePgmM>> GetRolePgmList(string id);

        public Task<ResponseResult<bool>> UpdateRolePgmFuncts(SysRoleRelationM model);
    }
}
