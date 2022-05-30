using AutoMapper;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Domain.Models;

namespace Hao.Authentication.Manager.Implements
{
    public class SysManager : BaseManager, ISysManager
    {
        private readonly ILogger _logger;
        public SysManager(PlatFormDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SysManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Add(SysM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Update(SysM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysM>> GetList(PagingParameter<SysFilter> param)
        {
            var res = new ResponsePagingResult<SysM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Delete(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysProgramM>> GetOwnedPgmList(PagingParameter<SysOwnedPgmFilter> param)
        {
            var res = new ResponsePagingResult<SysProgramM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysProgramM>> GetNotOwnedPgmList(PagingParameter<SysNotOwnedPgmFilter> param)
        {
            var res = new ResponsePagingResult<SysProgramM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysCtmM>> GetCtms(PagingParameter<SysCtmFilter> param)
        {
            var res = new ResponsePagingResult<SysCtmM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }



        public async Task<ResponseResult<bool>> AddRole(SysRoleM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> UpdateRole(SysRoleM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysRoleM>> GetRoleList(PagingParameter<SysRoleFilter> param)
        {
            var res = new ResponsePagingResult<SysRoleM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> DeleteRole(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysRolePgmM>> GetRolePgmList(string id)
        {
            var res = new ResponsePagingResult<SysRolePgmM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> UpdateRolePgmFuncts(SysRoleRelationM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }
    }
}
