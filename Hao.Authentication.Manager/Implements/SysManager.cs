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
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Common.Enums;

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

        #region System

        public async Task<ResponseResult<bool>> Add(SysM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                // 判断是否存在同名、同标识码的系统
                var nameExist = await _dbContext.Sys
                    .AnyAsync(x => !x.Deleted && x.Name == model.Name);
                if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的系统已存在！");
                var codeExist = await _dbContext.Sys
                    .AnyAsync(x => !x.Deleted && x.Code == model.Code);
                if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的系统已存在！");

                var entity = _mapper.Map<Sys>(model);
                entity.Id = entity.GetId(this.MachineCode);
                entity.CreatedById = CurrentUserId;

                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加系统【{model.Name} - {model.Code}】失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Update(SysM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Sys
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == model.Id);
                if (entity == null) throw new MyCustomException($"未查询到Id为【{model.Id}】的系统信息！");

                // 如果名称或标识码改变则判断是否与其他系统冲突
                if (entity.Name != model.Name)
                {
                    var nameExist = await _dbContext.Sys
                    .AnyAsync(x => !x.Deleted && x.Name == model.Name);
                    if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的系统已存在！");
                    entity.Name = model.Name;
                }
                if (entity.Code != model.Code)
                {
                    var codeExist = await _dbContext.Sys
                    .AnyAsync(x => !x.Deleted && x.Code == model.Code);
                    if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的系统已存在！");
                    entity.Code = model.Code;
                }
                entity.Intro = model.Intro;
                entity.Remark = model.Remark;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新系统【{model.Id}】失败");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysM>> GetList(PagingParameter<SysFilter> param)
        {
            var res = new ResponsePagingResult<SysM>();
            try
            {
                var query = _dbContext.SysView.AsQueryable();
                var filter = param.Filter;
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Name))
                        query = query.Where(x => x.Name.Contains(filter.Name));
                    if (!string.IsNullOrEmpty(filter.Code))
                        query = query.Where(x => x.Code.Contains(filter.Code));
                    if (!string.IsNullOrEmpty(filter.IntroOrRemark))
                        query = query.Where(x => x.Remark.Contains(filter.IntroOrRemark) || x.Intro.Contains(filter.IntroOrRemark));
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
                data.ForEach(x =>
                {
                    x.Logo = this.BuilderFileUrl(x.Logo);
                });
                res.Data = _mapper.Map<List<SysM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取系统列表失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Delete(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Sys
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);
                if (entity == null) throw new MyCustomException($"未查询到Id为【{id}】的系统信息！");

                entity.Deleted = true;
                entity.DeletedAt = DateTime.Now;
                entity.DeletedById = CurrentUserId;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"删除系统【{id}】信息失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<OptionItem<string>>> GetOptions()
        {
            var res = new ResponsePagingResult<OptionItem<string>>();
            try
            {
                res.Data = await _dbContext.Sys
                    .Where(x => !x.Deleted)
                    .OrderBy(x => x.Name)
                    .Select(y => new OptionItem<string>
                    {
                        Key = y.Id,
                        Value = y.Name
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取系统下拉选项失败！");
            }
            return res;
        }

        #endregion

        #region Program

        public async Task<ResponseResult<bool>> AddPgm(string sysId, string pgmId)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var exist = await _dbContext.SysProgramRelation
                    .AnyAsync(x => x.ProgramId == pgmId && x.SysId == sysId);
                if (exist) throw new MyCustomException("系统已关联该程序，请勿重复添加！");

                var entity = new SysProgramRelation
                {
                    SysId = sysId,
                    ProgramId = pgmId,
                    CreatedAt = DateTime.Now,
                    CreatedById = CurrentUserId,
                };

                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加系统【{sysId}】关联程序【{pgmId}】失败失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> DeletePgm(string sysId, string pgmId)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.SysProgramRelation
                    .FirstOrDefaultAsync(x => x.SysId == sysId && x.ProgramId == pgmId);
                if (entity == null) return res;
                _dbContext.Remove(entity);

                var roleIds = await _dbContext.SysRole
                    .Where(x => x.SysId == sysId)
                    .Select(y => y.Id)
                    .ToListAsync();
                if (roleIds.Any())
                {
                    var srfs = await _dbContext.SysRoleFuncRelation
                    .Where(x => x.ProgramId == pgmId && roleIds.Contains(x.RoleId))
                    .ToListAsync();
                    if (srfs.Any()) _dbContext.RemoveRange(srfs);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"取消系统【{sysId}】的程序【{pgmId}】关联失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysProgramM>> GetOwnedPgmList(PagingParameter<SysOwnedPgmFilter> param)
        {
            var res = new ResponsePagingResult<SysProgramM>();
            try
            {
                var filter = param.Filter;
                if (filter == null) throw new MyCustomException("查询参数未添加必要的系统信息");

                var query = from sp in _dbContext.SysProgramRelation
                            join p in _dbContext.Program on sp.ProgramId equals p.Id
                            where p.Deleted == false && sp.SysId == filter.SysId
                            select p;
                if (!string.IsNullOrEmpty(filter.NameOrCode))
                    query = query.Where(x => x.Name.Contains(filter.NameOrCode) || x.Code.Contains(filter.NameOrCode));
                if (!string.IsNullOrEmpty(filter.IntroOrRemark))
                    query = query.Where(x => x.Remark.Contains(filter.IntroOrRemark) || x.Intro.Contains(filter.IntroOrRemark));
                if (filter.Category.HasValue && filter.Category.Value != 0)
                    query = query.Where(x => x.Category == filter.Category);

                query = query.OrderBy(x => x.Name);
                var entities = await query.ToListAsync();
                res.RowsCount = entities.Count;
                var data = _mapper.Map<List<SysProgramM>>(entities);
                res.Data = data;
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"查询系统【{param.Filter?.SysId}】的关联程序列表失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysProgramM>> GetNotOwnedPgmList(PagingParameter<SysNotOwnedPgmFilter> param)
        {
            var res = new ResponsePagingResult<SysProgramM>();
            try
            {
                var filter = param.Filter;
                if (filter == null) throw new MyCustomException("查询参数未添加必要的系统信息");

                var query = from p in _dbContext.Program
                            join sp in _dbContext.SysProgramRelation on p.Id equals sp.ProgramId into sp1
                            from sp2 in sp1.DefaultIfEmpty()
                            where p.Deleted == false && sp2.SysId != filter.SysId
                            select p;

                if (!string.IsNullOrEmpty(filter.NameOrCode))
                    query = query.Where(x => x.Name.Contains(filter.NameOrCode) || x.Code.Contains(filter.NameOrCode));
                if (filter.Category.HasValue && filter.Category.Value != 0)
                    query = query.Where(x => x.Category == filter.Category.Value);

                query = query.OrderBy(x => x.Name);
                var entities = await query.ToListAsync();
                res.RowsCount = entities.Count;
                var data = _mapper.Map<List<SysProgramM>>(entities);
                res.Data = data;
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"查询系统【{param.Filter?.SysId}】的关联程序列表失败！");
            }
            return res;
        }

        #endregion

        #region Customer

        public async Task<ResponsePagingResult<SysCtmM>> GetCtms(PagingParameter<SysCtmFilter> param)
        {
            var res = new ResponsePagingResult<SysCtmM>();
            try
            {
                var filter = param.Filter;
                if (filter == null) throw new MyCustomException("查询参数未添加必要的系统信息");

                var query = _dbContext.SysCtmView.Where(x => x.SysId == filter.SysId);
                if (!string.IsNullOrEmpty(filter.NameOrRole))
                    query = query.Where(x => x.Name.Contains(filter.NameOrRole)
                    || (x.RoleName != null && x.RoleName.Contains(filter.NameOrRole)));
                if (!string.IsNullOrEmpty(filter.Remark))
                    query = query.Where(x => x.Remark.Contains(filter.Remark));
                if (filter.Limited.HasValue)
                    query = query.Where(x => x.Limited == filter.Limited.Value);
                if (filter.StartAt.HasValue)
                    query = query.Where(x => x.LastLoginAt >= filter.StartAt.Value);
                if (filter.EndAt.HasValue)
                    query = query.Where(x => x.LastLoginAt <= filter.EndAt.Value.AddDays(1).AddSeconds(-1));

                query = query.OrderByDescending(x => x.LastLoginAt);
                if (param.Sort != null && param.Sort.ToLower() == "desc")
                {
                    if (param.SortColumn?.ToLower() == "Name".ToLower())
                        query = query.OrderByDescending(x => x.Name);
                }
                else
                {
                    if (param.SortColumn?.ToLower() == "LastLoginAt".ToLower())
                        query = query.OrderBy(x => x.CreatedAt);
                    if (param.SortColumn?.ToLower() == "Name".ToLower())
                        query = query.OrderBy(x => x.Name);
                }

                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(param.PageIndex, param.PageSize);
                var data = await query.ToListAsync();
                res.Data = _mapper.Map<List<SysCtmM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"查询系统【{param.Filter?.SysId}】的客户列表失败！");
            }
            return res;
        }

        #endregion

        #region Role

        public async Task<ResponseResult<bool>> AddRole(SysRoleM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                // 判断该程序下是否存在同名、同标识码的页面/模块
                var nameExist = await _dbContext.SysRole
                    .AnyAsync(x => !x.Deleted && x.SysId == model.SysId && x.Name == model.Name);
                if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的角色已存在！");
                var codeExist = await _dbContext.SysRole
                    .AnyAsync(x => !x.Deleted && x.SysId == model.SysId && x.Code == model.Code);
                if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的角色已存在！");

                var entity = _mapper.Map<SysRole>(model);
                entity.Id = entity.GetId(this.MachineCode);
                entity.CreatedById = CurrentUserId;
                // TODO 约束
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加系统【{model.SysId}】的角色【 {model.Name}】失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> UpdateRole(SysRoleM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.SysRole
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == model.Id);
                if (entity == null) throw new MyCustomException($"未查询到Id为【{model.Id}】的角色信息！");

                // 如果名称或标识码改变则判断是否与其他程序冲突
                if (entity.Name != model.Name)
                {
                    var nameExist = await _dbContext.SysRole
                    .AnyAsync(x => !x.Deleted && x.SysId == model.SysId && x.Name == model.Name);
                    if (nameExist) throw new MyCustomException($"名称为【{model.Name}】的角色已存在！");
                    entity.Name = model.Name;
                }
                if (entity.Code != model.Code)
                {
                    var codeExist = await _dbContext.SysRole
                    .AnyAsync(x => !x.Deleted && x.SysId == model.SysId && x.Code == model.Code);
                    if (codeExist) throw new MyCustomException($"标识码为【{model.Code}】的角色已存在！");
                    entity.Code = model.Code;
                }

                entity.Intro = model.Intro;
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
                        Category = ConstraintCategory.system_role,
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
                _logger.LogError(e, $"更新角色【{model.Id}】失败");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysRoleM>> GetRoleList(PagingParameter<SysRoleFilter> param)
        {
            var res = new ResponsePagingResult<SysRoleM>();
            try
            {
                var filter = param.Filter;
                if (filter == null) throw new MyCustomException("查询参数未添加必要的系统信息");
                var query = _dbContext.SysRoleView.Where(x => x.SysId == filter.SysId);
                if (!string.IsNullOrEmpty(filter.NameOrCode))
                    query = query.Where(x => x.Name.Contains(filter.NameOrCode) || x.Code.Contains(filter.NameOrCode));
                if (filter.CttMethod.HasValue && filter.CttMethod != 0)
                    query = query.Where(x => x.CttMethod == filter.CttMethod.Value);
                if (filter.Rank.HasValue && filter.Rank != 0)
                    query = query.Where(x => x.Rank == filter.Rank.Value);
                if (filter.StartAt.HasValue)
                    query = query.Where(x => x.CreatedAt >= filter.StartAt.Value);
                if (filter.EndAt.HasValue)
                    query = query.Where(x => x.CreatedAt <= filter.EndAt.Value.AddDays(1).AddSeconds(-1));

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

                query = query.OrderBy(x => x.Name);
                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(param.PageIndex, param.PageSize);
                var entities = await query.ToListAsync();
                var data = _mapper.Map<List<SysRoleM>>(entities);
                res.Data = data;
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"查询系统【{param.Filter?.SysId}】的角色列表失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> DeleteRole(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.SysRole
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null) throw new MyCustomException("未查询到角色信息");
                entity.Deleted = true;
                entity.DeletedAt = DateTime.Now;
                entity.DeletedById = CurrentUserId;

                var srfs = await _dbContext.SysRoleFuncRelation
                    .Where(x => x.RoleId == id)
                    .ToListAsync();
                if (srfs.Any()) _dbContext.RemoveRange(srfs);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"删除id为【{id}】角色的信息失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<OptionItem<string>>> GetRoleOptions(string sysId)
        {
            var res = new ResponsePagingResult<OptionItem<string>>();
            try
            {
                var rank = SysRoleRank.super_manager;
                res.Data = await _dbContext.SysRole
                    .Where(x => !x.Deleted && x.SysId == sysId && x.Rank < rank)
                    .OrderBy(x => x.Name)
                    .Select(y => new OptionItem<string>
                    {
                        Key = y.Id,
                        Value = y.Name
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取角色下拉选项失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<SysRolePgmM>> GetRolePgmList(string id)
        {
            var res = new ResponsePagingResult<SysRolePgmM>();
            try
            {
                var pgms = await (from sr in _dbContext.SysRole
                                  join sp in _dbContext.SysProgramRelation on sr.SysId equals sp.SysId
                                  join p in _dbContext.Program on sp.ProgramId equals p.Id
                                  where sr.Id == id && !p.Deleted
                                  orderby p.Name
                                  select new SysRolePgmM
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                  }).ToListAsync();
                if (!pgms.Any()) return res;
                var pgmIds = pgms.Select(x => x.Id).ToList();

                var sects = await _dbContext.ProgramSection
                    .Where(x => !x.Deleted && pgmIds.Contains(x.ProgramId))
                    .OrderBy(x => x.Name)
                    .Select(y => new SysRoleSectM
                    {
                        Id = y.Id,
                        Name = y.Name,
                        PgmId = y.ProgramId,
                        Checked = false
                    }).ToListAsync();
                if (!sects.Any()){ res.Data = pgms;return res; }
                var sectIds = sects.Select(x => x.Id).ToList();

                var functs = await _dbContext.ProgramFunction
                    .Where(x => !x.Deleted && pgmIds.Contains(x.ProgramId))
                    .OrderBy(x => x.Name)
                    .Select(y => new SysRoleFunctM
                    {
                        Id = y.Id,
                        Name = y.Name,
                        PgmId = y.ProgramId,
                        SectId = y.SectionId,
                        Checked = false
                    }).ToListAsync();

                var checkIds = await _dbContext.SysRoleFuncRelation
                    .Where(x=>x.RoleId == id)
                    .Select(x => x.TargetId)
                    .ToListAsync();
                if (checkIds.Any())
                {
                    sects.ForEach(x =>
                    {
                        if (checkIds.Contains(x.Id)) x.Checked = true;
                    });
                    functs.ForEach(x =>
                    {
                        if (checkIds.Contains(x.Id)) x.Checked = true;
                    });
                }

                if(functs.Any())
                {
                    sects.ForEach(x =>
                    {
                        x.Functs = functs.Where(y=>y.SectId == x.Id).ToList();
                    });
                }
                pgms.ForEach(x =>
                {
                    x.Sects = sects.Where(y => y.PgmId == x.Id).ToList();
                });

                res.Data = pgms;
                res.RowsCount = pgms.Count;
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
                if (string.IsNullOrEmpty(model.PgmId) || string.IsNullOrEmpty(model.RoleId))
                    throw new MyCustomException("程序Id或RoleId为空！");
                var oldEntities = await _dbContext.SysRoleFuncRelation
                    .Where(x=>x.ProgramId == model.PgmId && x.RoleId == model.RoleId)
                    .ToListAsync();
                if(oldEntities.Any()) _dbContext.RemoveRange(oldEntities);

                var newEntities = new List<SysRoleFuncRelation>();
                if (model.SectIds != null)
                {
                    model.SectIds.ForEach(x =>
                    {
                        newEntities.Add(new SysRoleFuncRelation() { TargetId = x, IsFunction = false, });
                    });
                }
                if (model.FunctIds != null)
                {
                    model.FunctIds.ForEach(x =>
                    {
                        newEntities.Add(new SysRoleFuncRelation() { TargetId = x, IsFunction = true, });
                    });
                }
                newEntities.ForEach(x =>
                {
                    x.RoleId = model.RoleId;
                    x.ProgramId = model.PgmId;
                    x.CreatedAt = DateTime.Now;
                    x.CreatedById = CurrentUserId;
                });

                if(newEntities.Any()) await _dbContext.SysRoleFuncRelation.AddRangeAsync(newEntities);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"xxxx失败！");
            }
            return res;
        }

        #endregion Role
    }
}
