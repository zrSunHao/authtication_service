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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager _manager;

        public CustomerController(ICustomerManager manager)
        {
            _manager = manager;
        }

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

        [HttpPatch("AddRemark")]
        public async Task<ResponseResult<bool>> AddRemark(string ctmId, string remark)
        {
            return await _manager.AddRemark(ctmId, remark);
        }

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


        [HttpPost("GetRoleList")]
        public async Task<ResponsePagingResult<CtmRoleM>> GetRoleList(PagingParameter<CtmRoleFilter> param)
        {
            return await _manager.GetRoleList(param);
        }

        [HttpPost("AddRole")]
        public async Task<ResponseResult<bool>> AddRole(CtmRoleUpdateM model)
        {
            return await _manager.AddRole(model);
        }

        [HttpPatch("UpdateRole")]
        public async Task<ResponseResult<bool>> UpdateRole(CtmRoleUpdateM model)
        {
            return await _manager.UpdateRole(model);
        }

        [HttpDelete("DeleteRole")]
        public async Task<ResponseResult<bool>> DeleteRole(string ctmId, string roleId)
        {
            return await _manager.DeleteRole(ctmId, roleId);
        }



        [HttpPost("GetCttList")]
        public async Task<ResponsePagingResult<CttM>> GetCttList(PagingParameter<CtmCttFilter> param)
        {
            return await _manager.GetCttList(param);
        }

        [HttpPost("AddCtt")]
        public async Task<ResponseResult<bool>> AddCtt(CtmCttAddM model)
        {
            return await _manager.AddCtt(model);
        }

        [HttpPost("GetLogList")]
        public async Task<ResponsePagingResult<CtmLogM>> GetLogList(PagingParameter<CtmLogFilter> param)
        {
            return await _manager.GetLogList(param);
        }
    }
}
