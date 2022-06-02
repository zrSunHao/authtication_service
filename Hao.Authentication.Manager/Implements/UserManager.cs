using AutoMapper;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Persistence.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Manager.Implements
{
    public class UserManager : BaseManager, IUserManager
    {
        private readonly ILogger _logger;
        public UserManager(PlatFormDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<ResponseResult<AuthResultM>> Login(LoginM model)
        {
            var res = new ResponseResult<AuthResultM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"用户【{model.UserName}登录失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Register(RegisterM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"用户【{model.UserName}注册失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> ResetPsd(string oldPsd, string newPsd)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"id为【{CurrentUserId}的用户登录失败");
            }
            return res;
        }
    }
}
