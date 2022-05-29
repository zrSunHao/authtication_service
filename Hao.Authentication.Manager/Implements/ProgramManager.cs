using AutoMapper;
using Hao.Authentication.Common.Enums;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                // 判断是否存在同名、同标识码的程序
                var nameExist = await _dbContext.Program
                    .AnyAsync(x => !x.Deleted && x.Name == model.Name);
                if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的程序已存在！");
                var codeExist = await _dbContext.Program
                    .AnyAsync(x => !x.Deleted && x.Code == model.Code);
                if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的程序已存在！");

                var entity = _mapper.Map<Program>(model);
                entity.Id = entity.GetId(this.MachineCode);
                entity.CreatedById = CurrentUserId;

                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加程序【{model.Name} - {model.Code}】失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Update(ProgramM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Program
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == model.Id);
                if (entity == null) throw new MyCustomException($"未查询到Id为【{model.Id}】的程序信息！");

                // 如果名称或标识码改变则判断是否与其他程序冲突
                if (entity.Name != model.Name)
                {
                    var nameExist = await _dbContext.Program
                    .AnyAsync(x => !x.Deleted && x.Name == model.Name);
                    if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的程序已存在！");
                    entity.Name = model.Name;
                }
                if (entity.Code != model.Code)
                {
                    var codeExist = await _dbContext.Program
                    .AnyAsync(x => !x.Deleted && x.Code == model.Code);
                    if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的程序已存在！");
                    entity.Code = model.Code;
                }
                entity.Category = model.Category;
                entity.Intro = model.Intro;
                entity.Remark = model.Remark;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;

                await _dbContext.SaveChangesAsync();
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
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            try
            {
                var query = _dbContext.Program.Where(x => !x.Deleted);
                var filter = param.Filter;
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.NameOrCode))
                        query = query.Where(x => x.Name.Contains(filter.NameOrCode) || x.Code.Contains(filter.NameOrCode));
                    if (!string.IsNullOrEmpty(filter.IntroOrRemark))
                        query = query.Where(x => x.Remark.Contains(filter.IntroOrRemark) || x.Intro.Contains(filter.IntroOrRemark));
                    if (filter.Category.HasValue && filter.Category.Value != 0)
                        query = query.Where(x => x.Category == filter.Category);
                    if (filter.StartAt.HasValue)
                        query = query.Where(x => x.CreatedAt >= filter.StartAt.Value);
                    if (filter.EndAt.HasValue)
                        query = query.Where(x => x.CreatedAt <= filter.EndAt.Value.AddDays(1).AddSeconds(-1));
                }

                query = query.OrderByDescending(x => x.CreatedAt);
                if (param.Sort != null && param.Sort.ToLower() == "desc")
                {
                    if (param.SortColumn?.ToLower() == "Name".ToLower())
                        query = query.OrderByDescending(x => x.Name);
                }
                else
                {
                    if (param.SortColumn?.ToLower() == "CreatedAt".ToLower())
                        query = query.OrderBy(x => x.CreatedAt);
                    if (param.SortColumn?.ToLower() == "Name".ToLower())
                        query = query.OrderBy(x => x.Name);
                }

                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(param.PageIndex, param.PageSize);
                var data = await query.ToListAsync();
                res.Data = _mapper.Map<List<ProgramM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取程序列表失败");
            }
            watch1.Stop();
            Console.WriteLine($"方法耗时{watch1.ElapsedMilliseconds}毫秒");
            return res;
        }

        public async Task<ResponseResult<bool>> Delete(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var sysNames = await (from SPR in _dbContext.SysProgramRelation
                                      join S in _dbContext.Sys on SPR.SysId equals S.Id
                                      where SPR.ProgramId == id && S.Deleted == false
                                      select S.Name)
                                .ToListAsync();
                if (sysNames.Any())
                {
                    string namesMsg = string.Join("，", sysNames);
                    throw new MyCustomException($"该程序被以下【{sysNames.Count}】个系统使用，不能删除！ “{namesMsg}”");
                }

                var entity = await _dbContext.Program
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);
                if (entity == null) throw new MyCustomException($"未查询到Id为【{id}】的程序信息！");
                entity.Deleted = true;
                entity.DeletedAt = DateTime.Now;
                entity.DeletedById = CurrentUserId;

                await _dbContext.ProgramSection
                    .Where(x => x.ProgramId == id && !x.Deleted)
                    .ForEachAsync(x =>
                    {
                        x.Deleted = true;
                        x.DeletedAt = DateTime.Now;
                        x.DeletedById = CurrentUserId;
                    });

                await _dbContext.ProgramFunction
                    .Where(x => x.ProgramId == id && !x.Deleted)
                    .ForEachAsync(x =>
                    {
                        x.Deleted = true;
                        x.DeletedAt = DateTime.Now;
                        x.DeletedById = CurrentUserId;
                    });

                await _dbContext.SaveChangesAsync();
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
                var category = await GetSectionCategory(model.PgmId);
                var categoryName = category == SectionCategory.page ? "页面" : "模块";

                // 判断该程序下是否存在同名、同标识码的页面/模块
                var nameExist = await _dbContext.ProgramSection
                    .AnyAsync(x => !x.Deleted && x.ProgramId == model.PgmId && x.Name == model.Name);
                if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的{categoryName}已存在！");
                var codeExist = await _dbContext.ProgramSection
                    .AnyAsync(x => !x.Deleted && x.ProgramId == model.PgmId && x.Code == model.Code);
                if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的{categoryName}已存在！");

                var entity = _mapper.Map<ProgramSection>(model);
                entity.Id = entity.GetId(this.MachineCode);
                entity.Category = category;
                entity.CreatedById = CurrentUserId;

                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
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
                var category = await GetSectionCategory(model.PgmId);
                var categoryName = category == SectionCategory.page ? "页面" : "模块";

                var entity = await _dbContext.ProgramSection
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == model.Id);
                if (entity == null) throw new MyCustomException($"未查询到Id为【{model.Id}】的{categoryName}信息！");

                // 如果名称或标识码改变则判断是否与其他程序冲突
                if (entity.Name != model.Name)
                {
                    var nameExist = await _dbContext.ProgramSection
                    .AnyAsync(x => !x.Deleted && x.ProgramId == model.PgmId && x.Name == model.Name);
                    if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的{categoryName}已存在！");
                    entity.Name = model.Name;
                }
                if (entity.Code != model.Code)
                {
                    var codeExist = await _dbContext.ProgramSection
                    .AnyAsync(x => !x.Deleted && x.ProgramId == model.PgmId && x.Code == model.Code);
                    if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的{categoryName}已存在！");
                    entity.Code = model.Code;
                }
                entity.Category = category;
                entity.Remark = model.Remark;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;

                await _dbContext.SaveChangesAsync();
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
                var data = await _dbContext.ProgramSection
                    .Where(x => !x.Deleted)
                    .OrderBy(x => x.Code)
                    .ToListAsync();
                res.RowsCount = data.Count();
                res.Data = _mapper.Map<List<SectM>>(data);
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
                var entity = await _dbContext.ProgramSection
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);
                if (entity == null) throw new MyCustomException($"未查询到Id为【{id}】的页面/模块信息！");
                entity.Deleted = true;
                entity.DeletedAt = DateTime.Now;
                entity.DeletedById = CurrentUserId;

                await _dbContext.ProgramFunction
                    .Where(x => x.SectionId == id && !x.Deleted)
                    .ForEachAsync(x =>
                    {
                        x.Deleted = true;
                        x.DeletedAt = DateTime.Now;
                        x.DeletedById = CurrentUserId;
                    });

                await _dbContext.SaveChangesAsync();
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
                // 判断该页面/模块下是否存在同名、同标识码的功能
                var nameExist = await _dbContext.ProgramFunction
                    .AnyAsync(x => !x.Deleted && x.SectionId == model.SectId && x.Name == model.Name);
                if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的功能已存在！");
                var codeExist = await _dbContext.ProgramFunction
                    .AnyAsync(x => !x.Deleted && x.SectionId == model.SectId && x.Code == model.Code);
                if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的功能已存在！");

                var entity = _mapper.Map<ProgramFunction>(model);
                entity.Id = entity.GetId(this.MachineCode);
                entity.CreatedById = CurrentUserId;

                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
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
                var entity = await _dbContext.ProgramFunction
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == model.Id);
                if (entity == null) throw new MyCustomException($"未查询到Id为【{model.Id}】的程序信息！");

                // 如果名称或标识码改变则判断是否与其他程序冲突
                if (entity.Name != model.Name)
                {
                    var nameExist = await _dbContext.ProgramFunction
                    .AnyAsync(x => !x.Deleted && x.SectionId == model.SectId && x.Name == model.Name);
                    if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的功能已存在！");
                    entity.Name = model.Name;
                }
                if (entity.Code != model.Code)
                {
                    var codeExist = await _dbContext.ProgramFunction
                    .AnyAsync(x => !x.Deleted && x.SectionId == model.SectId && x.Code == model.Code);
                    if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的功能已存在！");
                    entity.Code = model.Code;
                }
                entity.Remark = model.Remark;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;

                await _dbContext.Constraint
                        .Where(x => !x.Cancelled && x.TargetId == model.Id)
                        .ForEachAsync(x => { x.Cancelled = true; });
                if (model.CttMethod != null && model.CttMethod != 0)
                {
                    var ctt = new Constraint
                    {
                        TargetId = model.Id,
                        Category = ConstraintCategory.program_function,
                        Method = model.CttMethod.Value,
                        ExpiredAt = model.CttMethod == ConstraintMethod._lock ? model.LimitedExpiredAt : null,
                        Origin = "管理员设置",
                        Remark = "无",
                        Cancelled = false,
                        CreatedAt = DateTime.Now,
                        CreatedById = this.CurrentUserId,
                    };
                    ctt.Id = ctt.GetId(this.MachineCode);
                    await _dbContext.AddAsync(ctt);
                }

                await _dbContext.SaveChangesAsync();
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
                var data = await _dbContext.PgmFunctView
                    .Where(x => x.SectId == sectId)
                    .OrderBy(x => x.Code)
                    .ToListAsync();
                res.Data = _mapper.Map<List<FunctM>>(data);
                res.RowsCount = data.Count;
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
                var entity = await _dbContext.ProgramFunction
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);
                if (entity == null) throw new MyCustomException($"未查询到Id为【{id}】的功能信息！");
                entity.Deleted = true;
                entity.DeletedAt = DateTime.Now;
                entity.DeletedById = CurrentUserId;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"删除功能【{id}】失败");
            }
            return res;
        }

        #endregion


        /// <summary>
        /// 获取页面/模块类型
        /// </summary>
        /// <param name="pgmId"></param>
        /// <returns></returns>
        /// <exception cref="MyCustomException"></exception>
        private async Task<SectionCategory> GetSectionCategory(string pgmId)
        {
            var entity = await _dbContext.Program
                   .FirstOrDefaultAsync(x => !x.Deleted && x.Id == pgmId);
            if (entity == null) throw new MyCustomException($"未查询到Id为【{pgmId}】的程序信息！");
            var category = SectionCategory.page;
            switch (entity.Category)
            {
                case ProgramCategory.web:
                case ProgramCategory.desktop:
                case ProgramCategory.mobile:
                    category = SectionCategory.page;
                    break;
                case ProgramCategory.service:
                    category = SectionCategory.module;
                    break;
            }
            return category;
        }
    }
}
