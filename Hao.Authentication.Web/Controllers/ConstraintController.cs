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
    public class ConstraintController : ControllerBase
    {
        private readonly IConstraintManager _manager;

        public ConstraintController(IConstraintManager manager)
        {
            _manager = manager;
        }

        [Function(CttFunct.Cancel)]
        [HttpDelete("Cancel")]
        public async Task<ResponseResult<bool>> Cancel(string id)
        {
            return await _manager.Cancel(id);
        }

        [Function(CttFunct.GetList)]
        [HttpPost("GetList")]
        public async Task<ResponsePagingResult<CttM>> GetList(PagingParameter<CttFilter> param)
        {
            return await _manager.GetList(param);
        }
    }
}
