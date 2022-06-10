using AutoMapper;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Manager.Providers;
using Hao.Authentication.Persistence.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Manager.Implements
{
    public class ReportManager : BaseManager, IReportManager
    {
        private readonly ILogger _logger;
        public ReportManager(PlatFormDbContext dbContext,
            IMapper mapper,
            ICacheProvider cache,
            IMyLogProvider myLog,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ReportManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor, cache, myLog)
        {
            _logger = logger;
        }

        public async Task<ResponsePagingResult<WidgetM>> GetWidgetList()
        {
            var res = new ResponsePagingResult<WidgetM>();
            try
            {
                var ctmCount = await _dbContext.Customer.CountAsync();
                var sysCount = await _dbContext.Sys.CountAsync(x => !x.Deleted);
                var pgmCount = await _dbContext.Program.CountAsync(x => !x.Deleted);
                var cttCount = await _dbContext.Constraint.CountAsync(x => !x.Cancelled && !(x.ExpiredAt!=null && x.ExpiredAt<=DateTime.Now));
                var list = new List<WidgetM>();
                list.Add(new WidgetM() { Msg = $"{ctmCount}名客户", Icon = "customer.png" });
                list.Add(new WidgetM() { Msg = $"{sysCount}个系统", Icon = "sys.png" });
                list.Add(new WidgetM() { Msg = $"{pgmCount}个程序", Icon = "program.png" });
                list.Add(new WidgetM() { Msg = $"{cttCount}个约束", Icon = "constraint.png" });
                res.Data = list;

                _myLog.Add(LoginRecord, "查看模块总量数据", "", RemoteIpAddress);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取展示数据失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<RecentLoginCtmM>> GetRecentLoginCtmList()
        {
            var res = new ResponsePagingResult<RecentLoginCtmM>();
            try
            {
                var data = await _dbContext.SysCtmView.AsNoTracking()
                    .Where(x=>x.LastLoginAt.HasValue)
                    .OrderByDescending(x => x.LastLoginAt)
                    .Select(x => new RecentLoginCtmM()
                    {
                        Avatar = x.Avatar,
                        Name = x.Name,
                        SysName = x.SysName,
                        LastLoginAt = x.LastLoginAt
                    })
                    .ToListAsync();
                data.ForEach(x =>
                {
                    x.Avatar = this.BuilderFileUrl(x.Avatar);
                });
                res.Data = data;
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取最近登录数据失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<RecentLogM>> GetRecentLogList()
        {
            var res = new ResponsePagingResult<RecentLogM>();
            try
            {
                res.Data = await (from l in _dbContext.CustomerLog
                                  join c in _dbContext.Customer on l.CustomerId equals c.Id
                                  join s in _dbContext.Sys on l.SystemId equals s.Id
                                  join r in _dbContext.SysRole on l.RoleId equals r.Id
                                  select new RecentLogM()
                                  {
                                      Name = c.Name,
                                      Operate = l.Operate,
                                      SysName = s.Name,
                                      RoleName = r.Name,
                                      CreatedAt = l.CreatedAt,
                                  }).ToListAsync();
                    
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取最近操作日志数据失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<CttM>> GetCtts()
        {
            var res = new ResponsePagingResult<CttM>();
            try
            {
                var data = await _dbContext.CttView.AsNoTracking()
                    .OrderByDescending(x => x.CreatedAt)
                    .AsPaging(1, 10)
                    .ToListAsync();
                res.Data = _mapper.Map<List<CttM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取最近约束数据失败！");
            }
            return res;
        }
    }
}
