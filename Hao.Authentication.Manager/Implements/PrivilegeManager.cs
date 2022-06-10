using AutoMapper;
using Hao.Authentication.Domain.Consts;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Manager.Providers;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Manager.Implements
{
    public class PrivilegeManager : BaseManager,IPrivilegeManager
    {
        private readonly ILogger _logger;
        
        public PrivilegeManager(PlatFormDbContext dbContext,
            IMapper mapper,
            ICacheProvider cache,
            IMyLogProvider myLog,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PrivilegeManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor, cache, myLog)
        {
            _logger = logger;
        }

        public async Task<UserLastLoginRecordM> GetLoginRecord(string? sid)
        {
            Guid loginId = Guid.Empty;
            if (string.IsNullOrEmpty(sid)) throw new Exception("登录Id为空！");
            else
            {
                Guid.TryParse(sid, out loginId);
                if (loginId == Guid.Empty) throw new Exception("登录Id格式化失败！");
            }
            string cacheKey = loginId.ToString();
            UserLastLoginRecordM? record = _cache.TryGetValue<UserLastLoginRecordM>(cacheKey);
            if (record == null)
            {
                var entity = await _dbContext.UserLastLoginRecord.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.LoginId == loginId);
                if (entity == null) throw new Exception("登录信息为空！");
                else
                {
                    record = _mapper.Map<UserLastLoginRecordM>(entity);
                    _cache.Save(cacheKey, record);
                }
            }
            if (record.ExpiredAt <= DateTime.Now) throw new Exception("登录信息已过期！");
            return record;
        }

        public async Task<List<string>> GetFunctCodes(string roleId)
        {
            string pgmCode = _configuration[CfgConsts.PLATFORM_PROGRAM_CODE];
            return await this.GetPgmFunctCodes(roleId,pgmCode);
        }

        public async Task<List<string>> GetPgmFunctCodes(string roleId,string pgmCode)
        {
            string cacheKey = CacheConsts.ROLE_PROGRAM_FUNCTS(roleId, pgmCode);
            List<string> codes = _cache.TryGetValue<List<string>>(cacheKey);
            if (codes == null)
                codes = await _dbContext.SysRoleFunctView.AsNoTracking()
                    .Where(x => x.Id == roleId && x.PgmCode == pgmCode && x.Limited != true)
                    .Select(x => x.FunctCode)
                    .ToListAsync();
            if (codes == null) codes = new List<string>();
            else _cache.Save(cacheKey, codes);
            return codes;
        }

        public async Task<List<string>> GetPgmSectCodes(string roleId, string pgmCode)
        {
            string cacheKey = CacheConsts.ROLE_PROGRAM_SECTS(roleId, pgmCode);
            List<string> codes = _cache.TryGetValue<List<string>>(cacheKey);
            if (codes == null)
                codes = await _dbContext.SysRoleSectView.AsNoTracking()
                    .Where(x => x.Id == roleId && x.PgmCode == pgmCode)
                    .Select(x => x.SectCode)
                    .ToListAsync();
            if (codes == null) codes = new List<string>();
            else _cache.Save(cacheKey, codes);
            return codes;
        }
    }
}
