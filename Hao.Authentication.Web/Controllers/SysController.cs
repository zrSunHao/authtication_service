using Hao.Authentication.Domain.Consts;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hao.Authentication.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SysController : ControllerBase
    {
        private readonly ISysManager _manager;

        public SysController(ISysManager manager)
        {
            _manager = manager;
        }

        [Function(SysFunct.Add)]
        [HttpPost("Add")]
        public async Task<ResponseResult<bool>> Add(SysM model)
        {
            return await _manager.Add(model);
        }

        [Function(SysFunct.Update)]
        [HttpPatch("Update")]
        public async Task<ResponseResult<bool>> Update(SysM model)
        {
            return await _manager.Update(model);
        }

        [Function(SysFunct.GetList)]
        [HttpPost("GetList")]
        public async Task<ResponsePagingResult<SysM>> GetList(PagingParameter<SysFilter> param)
        {
            return await _manager.GetList(param);
        }

        [Function(SysFunct.Delete)]
        [HttpDelete("Delete")]
        public async Task<ResponseResult<bool>> Delete(string id)
        {
            return await _manager.Delete(id);
        }

        [HttpGet("GetOptions")]
        public async Task<ResponsePagingResult<OptionItem<string>>> GetOptions()
        {
            return await _manager.GetOptions();
        }


        [Function(SysFunct.GetOwnedPgmList)]
        [HttpPost("GetOwnedPgmList")]
        public async Task<ResponsePagingResult<SysProgramM>> GetOwnedPgmList(PagingParameter<SysOwnedPgmFilter> param)
        {
            return await _manager.GetOwnedPgmList(param);
        }

        [Function(SysFunct.GetNotOwnedPgmList)]
        [HttpPost("GetNotOwnedPgmList")]
        public async Task<ResponsePagingResult<SysProgramM>> GetNotOwnedPgmList(PagingParameter<SysNotOwnedPgmFilter> param)
        {
            return await _manager.GetNotOwnedPgmList(param);
        }

        [Function(SysFunct.AddPgm)]
        [HttpPost("AddPgm")]
        public async Task<ResponseResult<bool>> AddPgm(string sysId, string pgmId)
        {
            return await _manager.AddPgm(sysId, pgmId);
        }

        [Function(SysFunct.DeletePgm)]
        [HttpDelete("DeletePgm")]
        public async Task<ResponseResult<bool>> DeletePgm(string sysId, string pgmId)
        {
            return await _manager.DeletePgm(sysId, pgmId);
        }


        [Function(SysFunct.GetCtms)]
        [HttpPost("GetCtms")]
        public async Task<ResponsePagingResult<SysCtmM>> GetCtms(PagingParameter<SysCtmFilter> param)
        {
            return await _manager.GetCtms(param);
        }

        [Function(SysFunct.AddCtmCtt)]
        [HttpPost("AddCtmCtt")]
        public async Task<ResponseResult<bool>> AddCtmCtt(CtmCttAddM model)
        {
            return await _manager.AddCtmCtt(model);
        }

        [Function(SysFunct.CancelCtmCtt)]
        [HttpDelete("CancelCtmCtt")]
        public async Task<ResponseResult<bool>> CancelCtmCtt(string sysId, string ctmId)
        {
            return await _manager.CancelCtmCtt(sysId, ctmId);
        }

        [Function(SysFunct.AddRole)]
        [HttpPost("AddRole")]
        public async Task<ResponseResult<bool>> AddRole(SysRoleM model)
        {
            return await _manager.AddRole(model);
        }

        [Function(SysFunct.UpdateRole)]
        [HttpPatch("UpdateRole")]
        public async Task<ResponseResult<bool>> UpdateRole(SysRoleM model)
        {
            return await _manager.UpdateRole(model);
        }

        [Function(SysFunct.GetRoleList)]
        [HttpPost("GetRoleList")]
        public async Task<ResponsePagingResult<SysRoleM>> GetRoleList(PagingParameter<SysRoleFilter> param)
        {
            return await _manager.GetRoleList(param);
        }

        [Function(SysFunct.DeleteRole)]
        [HttpDelete("DeleteRole")]
        public async Task<ResponseResult<bool>> DeleteRole(string id)
        {
            return await _manager.DeleteRole(id);
        }

        [HttpGet("GetRoleOptions")]
        public async Task<ResponsePagingResult<OptionItem<string>>> GetRoleOptions(string sysId)
        {
            return await _manager.GetRoleOptions(sysId);
        }

        [Function(SysFunct.GetRolePgmList)]
        [HttpGet("GetRolePgmList")]
        public async Task<ResponsePagingResult<SysRolePgmM>> GetRolePgmList(string id)
        {
            return await _manager.GetRolePgmList(id);
        }

        [Function(SysFunct.UpdateRolePgmFuncts)]
        [HttpPut("UpdateRolePgmFuncts")]
        public async Task<ResponseResult<bool>> UpdateRolePgmFuncts(SysRoleRelationM model)
        {
            return await _manager.UpdateRolePgmFuncts(model);
        }

        [AllowAnonymous]
        [HttpPut("Initial")]
        public async Task<ResponseResult<bool>> Initial(string psd)
        {
            return await _manager.Initial(psd);
        }
    }
}
