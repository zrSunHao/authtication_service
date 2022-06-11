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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager _manager;

        public CustomerController(ICustomerManager manager)
        {
            _manager = manager;
        }

        [Function(CtmFunct.GetList)]
        [HttpPost("GetList")]
        public async Task<ResponsePagingResult<CtmM>> GetList(PagingParameter<CtmFilter> param)
        {
            return await _manager.GetList(param);
        }

        [HttpGet("GetById")]
        public async Task<ResponseResult<CtmM>> GetById(string ctmId)
        {
            return await _manager.GetById(ctmId);
        }

        [Function(CtmFunct.AddRemark)]
        [HttpPatch("AddRemark")]
        public async Task<ResponseResult<bool>> AddRemark(string ctmId, string remark)
        {
            return await _manager.AddRemark(ctmId, remark);
        }

        [Function(CtmFunct.ResetPsd)]
        [HttpPatch("ResetPsd")]
        public async Task<ResponseResult<bool>> ResetPsd(string ctmId, string psd)
        {
            return await _manager.ResetPsd(ctmId, psd);
        }

        [HttpGet("GetPeople")]
        public async Task<ResponseResult<PeopleM>> GetPeople(string ctmId)
        {
            return await _manager.GetPeople(ctmId);
        }

        [HttpPatch("UpdatePeople")]
        public async Task<ResponseResult<bool>> UpdatePeople(PeopleM model)
        {
            return await _manager.UpdatePeople(model);
        }


        [Function(CtmFunct.GetRoleList)]
        [HttpPost("GetRoleList")]
        public async Task<ResponsePagingResult<CtmRoleM>> GetRoleList(PagingParameter<CtmRoleFilter> param)
        {
            return await _manager.GetRoleList(param);
        }

        [Function(CtmFunct.AddRole)]
        [HttpPost("AddRole")]
        public async Task<ResponseResult<bool>> AddRole(CtmRoleUpdateM model)
        {
            return await _manager.AddRole(model);
        }

        [Function(CtmFunct.UpdateRole)]
        [HttpPatch("UpdateRole")]
        public async Task<ResponseResult<bool>> UpdateRole(CtmRoleUpdateM model)
        {
            return await _manager.UpdateRole(model);
        }

        [Function(CtmFunct.DeleteRole)]
        [HttpDelete("DeleteRole")]
        public async Task<ResponseResult<bool>> DeleteRole(string ctmId, string roleId)
        {
            return await _manager.DeleteRole(ctmId, roleId);
        }



        [Function(CtmFunct.GetCttList)]
        [HttpPost("GetCttList")]
        public async Task<ResponsePagingResult<CttM>> GetCttList(PagingParameter<CtmCttFilter> param)
        {
            return await _manager.GetCttList(param);
        }

        [Function(CtmFunct.AddCtt)]
        [HttpPost("AddCtt")]
        public async Task<ResponseResult<bool>> AddCtt(CtmCttAddM model)
        {
            return await _manager.AddCtt(model);
        }

        [Function(CtmFunct.GetLogList)]
        [HttpPost("GetLogList")]
        public async Task<ResponsePagingResult<CtmLogM>> GetLogList(PagingParameter<CtmLogFilter> param)
        {
            return await _manager.GetLogList(param);
        }
    }
}
