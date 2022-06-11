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
    public class UserController : ControllerBase
    {
        private readonly IUserManager _manager;

        public UserController(IUserManager manager)
        {
            _manager = manager;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ResponseResult<AuthResultM>> Login(LoginM model)
        {
            return await _manager.Login(model);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ResponseResult<bool>> Register(RegisterM model)
        {
            return await _manager.Register(model);
        }

        [HttpPatch("ResetPsd")]
        public async Task<ResponseResult<bool>> ResetPsd(string oldPsd, string newPsd)
        {
            return await _manager.ResetPsd(oldPsd, newPsd);
        }

        [HttpDelete("Logout")]
        public async Task<ResponseResult<bool>> Logout()
        {
            return await _manager.Logout();
        }

        [HttpPost("AddLog")]
        public void AddLog(string operate, string remark = "")
        {
            _manager.AddLog(operate, remark);
        }

        [HttpPost("GetLogList")]
        public async Task<ResponsePagingResult<UserLogM>> GetLogList(PagingParameter<UserLogFilter> param)
        {
            return await _manager.GetLogList(param);
        }
    }
}
