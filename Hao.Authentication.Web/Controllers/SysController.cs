using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
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

        [HttpPost("Add")]
        public async Task<ResponseResult<bool>> Add(SysM model)
        {
            return await _manager.Add(model);
        }

        [HttpPatch("Update")]
        public async Task<ResponseResult<bool>> Update(SysM model)
        {
            return await _manager.Update(model);
        }

        [HttpPost("GetList")]
        public async Task<ResponsePagingResult<SysM>> GetList(PagingParameter<SysFilter> param)
        {
            return await _manager.GetList(param);
        }

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


        [HttpPost("GetOwnedPgmList")]
        public async Task<ResponsePagingResult<SysProgramM>> GetOwnedPgmList(PagingParameter<SysOwnedPgmFilter> param)
        {
            return await _manager.GetOwnedPgmList(param);
        }

        [HttpPost("GetNotOwnedPgmList")]
        public async Task<ResponsePagingResult<SysProgramM>> GetNotOwnedPgmList(PagingParameter<SysNotOwnedPgmFilter> param)
        {
            return await _manager.GetNotOwnedPgmList(param);
        }

        [HttpPost("AddPgm")]
        public async Task<ResponseResult<bool>> AddPgm(string sysId, string pgmId)
        {
            return await _manager.AddPgm(sysId, pgmId);
        }

        [HttpDelete("DeletePgm")]
        public async Task<ResponseResult<bool>> DeletePgm(string sysId, string pgmId)
        {
            return await _manager.DeletePgm(sysId, pgmId);
        }


        [HttpPost("GetCtms")]
        public async Task<ResponsePagingResult<SysCtmM>> GetCtms(PagingParameter<SysCtmFilter> param)
        {
            return await _manager.GetCtms(param);
        }


        [HttpPost("AddRole")]
        public async Task<ResponseResult<bool>> AddRole(SysRoleM model)
        {
            return await _manager.AddRole(model);
        }

        [HttpPatch("UpdateRole")]
        public async Task<ResponseResult<bool>> UpdateRole(SysRoleM model)
        {
            return await _manager.UpdateRole(model);
        }

        [HttpPost("GetRoleList")]
        public async Task<ResponsePagingResult<SysRoleM>> GetRoleList(PagingParameter<SysRoleFilter> param)
        {
            return await _manager.GetRoleList(param);
        }

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

        [HttpGet("GetRolePgmList")]
        public async Task<ResponsePagingResult<SysRolePgmM>> GetRolePgmList(string id)
        {
            return await _manager.GetRolePgmList(id);
        }

        [HttpPut("UpdateRolePgmFuncts")]
        public async Task<ResponseResult<bool>> UpdateRolePgmFuncts(SysRoleRelationM model)
        {
            return await _manager.UpdateRolePgmFuncts(model);
        }
    }
}
