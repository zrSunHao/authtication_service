using AutoMapper;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hao.Authentication.Manager.Implements
{
    public class CustomerManager : BaseManager, ICustomerManager
    {
        private readonly ILogger _logger;
        private readonly IConstraintManager _ctt;
        public CustomerManager(PlatFormDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IConstraintManager constraintManager,
            ILogger<CustomerManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
            _ctt = constraintManager;
        }

        public async Task<ResponsePagingResult<CtmM>> GetList(PagingParameter<CtmFilter> param)
        {
            var res = new ResponsePagingResult<CtmM>();
            try
            {
                var query = _dbContext.CtmView.AsQueryable();
                var filter = param.Filter;
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.NameOrNickname))
                        query = query.Where(x => x.Name.Contains(filter.NameOrNickname) || x.Nickname.Contains(filter.NameOrNickname));
                    if (!string.IsNullOrEmpty(filter.Remark))
                        query = query.Where(x => x.Remark != null && x.Remark.Contains(filter.Remark));
                    if (filter.Limited.HasValue)
                    {
                        if (filter.Limited.Value)
                            query = query.Where(x => x.Limited == true);
                        else query = query.Where(x => x.Limited == false);
                    }
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
                    if (param.SortColumn?.ToLower() == "LastLoginAt".ToLower())
                        query = query.OrderByDescending(x => x.LastLoginAt);
                }
                else
                {
                    if (param.SortColumn?.ToLower() == "CreatedAt".ToLower())
                        query = query.OrderBy(x => x.CreatedAt);
                    if (param.SortColumn?.ToLower() == "Name".ToLower())
                        query = query.OrderBy(x => x.Name);
                    if (param.SortColumn?.ToLower() == "LastLoginAt".ToLower())
                        query = query.OrderBy(x => x.LastLoginAt);
                }

                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(param.PageIndex, param.PageSize);
                var data = await query.ToListAsync();
                data.ForEach(x =>
                {
                    x.Avatar = this.BuilderFileUrl(x.Avatar);
                });
                res.Data = _mapper.Map<List<CtmM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取客户列表失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> AddRemark(string ctmId, string remark)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Customer
                    .FirstOrDefaultAsync(x => x.Id == ctmId);
                if (entity == null) throw new MyCustomException("未查询到客户数据！");
                entity.Remark = remark;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加Id为【{ctmId}】的客户的备注失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> ResetPsd(string ctmId, string psd)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Customer
                    .FirstOrDefaultAsync(x => x.Id == ctmId);
                if (entity == null) throw new MyCustomException("未查询到客户数据！");
                HashHandler.CreateHash(psd, out var hash, out var salt);
                entity.Password = hash;
                entity.PasswordSalt = salt;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"重置Id为【{ctmId}】的客户的密码失败");
            }
            return res;
        }

        public async Task<ResponseResult<CtmM>> GetById(string ctmId)
        {
            var res = new ResponseResult<CtmM>();
            try
            {
                var entity = await _dbContext.CtmView.AsNoTracking()
                   .FirstOrDefaultAsync(x => x.Id == ctmId);
                if (entity == null) throw new MyCustomException("未查询到客户数据！");
                entity.Avatar = this.BuilderFileUrl(entity.Avatar);
                res.Data = _mapper.Map<CtmM>(entity);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取Id为【{ctmId}】的客户账号信息失败");
            }
            return res;
        }

        public async Task<ResponseResult<PeopleM>> GetPeople(string ctmId)
        {
            var res = new ResponseResult<PeopleM>();
            try
            {
                var entity = await _dbContext.CustomerInformation
                   .FirstOrDefaultAsync(x => x.CustomerId == ctmId);
                if (entity == null) throw new MyCustomException("未查询到客户数据！");
                res.Data = _mapper.Map<PeopleM>(entity);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取Id为【{ctmId}】的客户个人信息失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> UpdatePeople(PeopleM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.CustomerInformation
                    .FirstOrDefaultAsync(x => x.CustomerId == model.CtmId);
                if (entity == null) throw new MyCustomException("未查询到客户数据！");
                entity.FullName = model.FullName;
                entity.Gender = model.Gender;
                entity.Birthday = model.Birthday;
                entity.Education = model.Education;
                entity.Profession = model.Profession;
                entity.Intro = model.Intro;
                entity.LastModifiedAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新客户Id为【{model.CtmId}】的个人信息失败");
            }
            return res;
        }


        public async Task<ResponsePagingResult<CtmRoleM>> GetRoleList(PagingParameter<CtmRoleFilter> param)
        {
            var res = new ResponsePagingResult<CtmRoleM>();
            try
            {
                var filter = param.Filter;
                if (filter == null || string.IsNullOrEmpty(filter?.CtmId))
                    throw new MyCustomException("缺少查询需要的客户信息！");
                var query = _dbContext.CtmRoleView.Where(x=>x.Id == filter.CtmId).AsNoTracking();

                if (!string.IsNullOrEmpty(filter.SysName))
                    query = query.Where(x => x.SysName.Contains(filter.SysName));
                if (!string.IsNullOrEmpty(filter.RoleName))
                    query = query.Where(x => x.RoleName.Contains(filter.RoleName));

                if (param.Sort != null && param.Sort.ToLower() == "desc")
                {
                    if (param.SortColumn?.ToLower() == "CreatedAt".ToLower())
                        query = query.OrderByDescending(x => x.CreatedAt);
                    if (param.SortColumn?.ToLower() == "SysName".ToLower())
                        query = query.OrderByDescending(x => x.SysName);
                }
                else
                {
                    if (param.SortColumn?.ToLower() == "CreatedAt".ToLower())
                        query = query.OrderBy(x => x.CreatedAt);
                    if (param.SortColumn?.ToLower() == "SysName".ToLower())
                        query = query.OrderBy(x => x.SysName);
                }

                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(1, 10);
                var data = await query.ToListAsync();
                data.ForEach(x =>
                {
                    x.SysLogo = this.BuilderFileUrl(x.SysLogo);
                });
                res.Data = _mapper.Map<List<CtmRoleM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取id为【{param.Filter?.CtmId}】的客户角色列表失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> AddRole(CtmRoleUpdateM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var exist = await _dbContext.CustomerRoleRelation
                    .AnyAsync(x => x.SysId == model.SysId && x.CustomerId == model.CtmId);
                if (exist) throw new MyCustomException("客户在该系统下已赋予角色！");

                var entity = new CustomerRoleRelation()
                {
                    CustomerId = model.CtmId,
                    RoleId = model.RoleId,
                    Remark = "暂无",
                    SysId = model.SysId,
                    CreatedAt = DateTime.Now,
                    CreatedById = CurrentUserId
                };
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新客户【{model.CtmId}】角色【{model.RoleId}】失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> UpdateRole(CtmRoleUpdateM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var olds = await _dbContext.CustomerRoleRelation
                    .Where(x => x.SysId == model.SysId && x.CustomerId == model.CtmId)
                    .ToListAsync();
                if (olds.Any()) _dbContext.RemoveRange(olds);

                var entity = new CustomerRoleRelation()
                {
                    CustomerId = model.CtmId,
                    RoleId = model.RoleId,
                    Remark = "暂无",
                    SysId = model.SysId,
                    CreatedAt = DateTime.Now,
                    CreatedById = CurrentUserId
                };
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新客户【{model.CtmId}】角色【{model.RoleId}】失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> DeleteRole(string ctmId, string roleId)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var olds = await _dbContext.CustomerRoleRelation
                    .Where(x => x.RoleId == roleId && x.CustomerId == ctmId)
                    .ToListAsync();
                if (olds.Any())
                {
                    _dbContext.RemoveRange(olds);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"删除客户【{ctmId}】角色【{roleId}】失败");
            }
            return res;
        }


        public async Task<ResponsePagingResult<CttM>> GetCttList(PagingParameter<CtmCttFilter> param)
        {
            var res = new ResponsePagingResult<CttM>();
            try
            {
                var filter = param.Filter;
                if (filter == null || string.IsNullOrEmpty(filter?.CtmId))
                    throw new MyCustomException("缺少查询需要的客户信息！");
                var query = _dbContext.CttView.AsNoTracking().Where(x=>x.CtmId == filter.CtmId);
                if (!string.IsNullOrEmpty(filter.SysName))
                    query = query.Where(x => x.SysName != null && x.Origin.Contains(filter.SysName));
                if (filter.Category.HasValue && filter.Category.Value != 0)
                    query = query.Where(x => x.Category == filter.Category);

                query = query.OrderByDescending(x => x.CreatedAt);
                if (param.Sort != null && param.Sort.ToLower() == "desc")
                {
                    if (param.SortColumn?.ToLower() == "ExpiredAt".ToLower())
                        query = query.OrderByDescending(x => x.ExpiredAt);
                }
                else
                {
                    if (param.SortColumn?.ToLower() == "CreatedAt".ToLower())
                        query = query.OrderBy(x => x.CreatedAt);
                    if (param.SortColumn?.ToLower() == "ExpiredAt".ToLower())
                        query = query.OrderBy(x => x.ExpiredAt);
                }

                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(param.PageIndex, param.PageSize);
                var data = await query.ToListAsync();
                res.Data = _mapper.Map<List<CttM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取id为【{param.Filter?.CtmId}】的客户约束列表失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> AddCtt(CtmCttAddM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                if (string.IsNullOrEmpty(model.Remark)) model.Remark = "无";
                var m = _mapper.Map<CttAddM>(model);
                var result = await _ctt.Add(m, false);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加客户【{model.CtmId}】系统【{model.SysId}】的约束失败");
            }
            return res;
        }

        public async Task<ResponsePagingResult<CtmLogM>> GetLogList(PagingParameter<CtmLogFilter> param)
        {
            var res = new ResponsePagingResult<CtmLogM>();
            try
            {
                var filter = param.Filter;
                if (filter == null) throw new MyCustomException("缺少查询需要的客户信息！");
                var query = from cl in _dbContext.CustomerLog
                            join s in _dbContext.Sys on cl.SystemId equals s.Id
                            join r in _dbContext.SysRole on cl.RoleId equals r.Id
                            join p in _dbContext.Program on cl.ProgramId equals p.Id
                            where cl.CustomerId == filter.CtmId
                            select new CtmLogM
                            {
                                Id = cl.Id,
                                Operate = cl.Operate,
                                SysName = s.Name,
                                PgmName = p.Name,
                                RoleName = r.Name,
                                CreatedAt = cl.CreatedAt,
                                Remark = cl.Remark,
                            };
                if (!string.IsNullOrEmpty(filter.Operate))
                    query = query.Where(x => x.Operate.Contains(filter.Operate));
                if (!string.IsNullOrEmpty(filter.SysName))
                    query = query.Where(x => x.SysName.Contains(filter.SysName));
                if (filter.StartAt.HasValue)
                    query = query.Where(x => x.CreatedAt >= filter.StartAt.Value);
                if (filter.EndAt.HasValue)
                    query = query.Where(x => x.CreatedAt <= filter.EndAt.Value.AddDays(1).AddSeconds(-1));

                query = query.OrderByDescending(x => x.CreatedAt);
                if (param.Sort != null && param.Sort.ToLower() == "desc")
                {
                    if (param.SortColumn?.ToLower() == "Operate".ToLower())
                        query = query.OrderByDescending(x => x.Operate);
                }
                else
                {
                    if (param.SortColumn?.ToLower() == "CreatedAt".ToLower())
                        query = query.OrderBy(x => x.CreatedAt);
                    if (param.SortColumn?.ToLower() == "Operate".ToLower())
                        query = query.OrderBy(x => x.Operate);
                }

                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(param.PageIndex, param.PageSize);
                res.Data = await query.ToListAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取id为【{param.Filter?.CtmId}】的客户日志列表失败");
            }
            return res;
        }
    }
}
