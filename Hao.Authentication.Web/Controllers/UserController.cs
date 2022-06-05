﻿using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hao.Authentication.Web.Controllers
{
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
            {
                return await _manager.Register(model);
            }
        }

        [HttpPatch("ResetPsd")]
        public async Task<ResponseResult<bool>> ResetPsd(string oldPsd, string newPsd)
        {
            {
                return await _manager.ResetPsd(oldPsd, newPsd);
            }
        }
    }
}