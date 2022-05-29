using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Microsoft.AspNetCore.Mvc;

namespace Hao.Authentication.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConstraintController : ControllerBase
    {
        private readonly IConstraintManager _manager;

        public ConstraintController(IConstraintManager manager)
        {
            _manager = manager;
        }

        [HttpDelete("Cancel")]
        public async Task<ResponseResult<bool>> Cancel(string id)
        {
            return await _manager.Cancel(id);
        }

        [HttpPost("GetList")]
        public async Task<ResponsePagingResult<CttM>> GetList(PagingParameter<CttFilter> param)
        {
            return await _manager.GetList(param);
        }
    }
}
