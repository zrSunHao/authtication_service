using AutoMapper;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
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
    public class ProgramManager : BaseManager, IProgramManager
    {
        private readonly ILogger _logger;
        public ProgramManager(PlatFormDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor,
            ILogger<ProgramManager> logger) 
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        #region Program

        public async Task<ResponseResult<bool>> Add(ProgramM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = _mapper.Map<Program>(model);
                entity.Id = entity.GetId(this.MachineCode);
                entity.CreatedById = CurrentUserId;
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e,$"添加程序【{model.Name} - {model.Code}】失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Update(ProgramM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新程序【{model.Id}】失败");
            }
            return res;
        }

        public async Task<ResponsePagingResult<ProgramM>> GetList(PagingParameter<PgmFilter> param)
        {
            var res = new ResponsePagingResult<ProgramM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取程序列表失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Delete(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var list = _dbContext.Program.Where(x => !x.Deleted);
                int t = list.Count();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"删除程序【{id}】失败");
            }
            return res;
        }

        #endregion

        #region Section

        public async Task<ResponseResult<bool>> AddSect(SectM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加页面/模块【{model.Name} - {model.Code}】失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> UpdateSect(SectM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新页面/模块【{model.Id}】失败");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SectM>> GetSectList(string pgmId)
        {
            var res = new ResponsePagingResult<SectM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取页面/模块列表失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> DeleteSect(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"删除页面/模块【{id}】失败");
            }
            return res;
        }

        #endregion

        #region Function

        public async Task<ResponseResult<bool>> AddFunct(FunctM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加功能【{model.Name} - {model.Code}】失败");
            }
            return res;
        }
        public async Task<ResponseResult<bool>> UpdateFunct(FunctM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新功能【{model.Id}】失败");
            }
            return res;
        }

        public async Task<ResponsePagingResult<FunctM>> GetFunctList(string sectId)
        {
            var res = new ResponsePagingResult<FunctM>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取页面/模块的【{sectId}】功能列表失败");
            }
            return res;
        }
        public async Task<ResponseResult<bool>> DeleteFunct(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {

            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"删除功能【{id}】失败");
            }
            return res;
        }

        #endregion
    }
}
