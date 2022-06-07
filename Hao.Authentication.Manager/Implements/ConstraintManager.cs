using AutoMapper;
using Hao.Authentication.Common.Enums;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Manager.Providers;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hao.Authentication.Manager.Implements
{
    public class ConstraintManager : BaseManager, IConstraintManager
    {
        private readonly ILogger _logger;
        public ConstraintManager(PlatFormDbContext dbContext,
            IMapper mapper,
            ICacheProvider cache,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ConstraintManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor, cache)
        {
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Add(CttAddM ctt, bool directlySave = false)
        {
            var res = new ResponseResult<bool>();
            try
            {
                if (string.IsNullOrEmpty(ctt.TargetId) || ctt.Category == 0 || ctt.Method == 0)
                    throw new MyCustomException("添加约束需要的信息不全！");

                    await _dbContext.Constraint
                    .Where(x => !x.Cancelled && x.TargetId == ctt.TargetId 
                    && x.Category == ctt.Category && x.SysId == ctt.SysId)
                    .ForEachAsync(y =>
                    {
                        y.Cancelled = true;
                        y.LastModifiedAt = DateTime.Now;
                        y.LastModifiedById = CurrentUserId;
                    });

                if (ctt.Method == ConstraintMethod._lock && (ctt.ExpiredAt == null
                    || (ctt.ExpiredAt.HasValue && ctt.ExpiredAt <= DateTime.Now)))
                    ctt.Method = ConstraintMethod.forbid;
                if (ctt.Category == ConstraintCategory.customer_one_system && string.IsNullOrEmpty(ctt.SysId))
                    ctt.Category = ConstraintCategory.customer_all_system;
                var entity = _mapper.Map<Constraint>(ctt);
                entity.Id = entity.GetId(MachineCode);
                entity.CreatedById = CurrentUserId;
                await _dbContext.AddAsync(entity);
                if(directlySave) await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加TargetId为【{ctt.TargetId}】、类别为【{ctt.Category.ToString()}】的约束失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Cancel(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Constraint
                    .Where(c => c.Id == id && !c.Cancelled)
                    .FirstOrDefaultAsync();
                if(entity == null) throw new MyCustomException($"未查询到Id为【{id}】的约束信息！");
                entity.Cancelled = true;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"取消Id为【{id}】的约束失败");
            }
            return res;
        }

        public async Task<ResponsePagingResult<CttM>> GetList(PagingParameter<CttFilter> param)
        {
            var res = new ResponsePagingResult<CttM>();
            try
            {
                var query = _dbContext.CttView.AsNoTracking().AsQueryable();
                var filter = param.Filter;
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Name))
                        query = query.Where(x => (x.CtmName != null && x.CtmName.Contains(filter.Name))
                        || (x.SysName != null && x.SysName.Contains(filter.Name)) 
                        || (x.FunctName != null && x.FunctName.Contains(filter.Name)));
                    if (!string.IsNullOrEmpty(filter.OriginOrRemark))
                        query = query.Where(x => x.Remark.Contains(filter.OriginOrRemark) 
                        || x.Origin.Contains(filter.OriginOrRemark));
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
                    if (param.SortColumn?.ToLower() == "expiredAt".ToLower())
                        query = query.OrderByDescending(x => x.ExpiredAt);
                }
                else
                {
                    if (param.SortColumn?.ToLower() == "CreatedAt".ToLower())
                        query = query.OrderBy(x => x.CreatedAt);
                    if (param.SortColumn?.ToLower() == "expiredAt".ToLower())
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
                _logger.LogError(e, $"获取约束列表失败");
            }
            return res;
        }
    }
}
