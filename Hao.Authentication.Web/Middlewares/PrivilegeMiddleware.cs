using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Manager.Providers;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;
using Hao.Authentication.Web.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hao.Authentication.Web.Middlewares
{
    public class PrivilegeMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<PrivilegeMiddleware> _logger;
        private PlatFormDbContext _dbContext;
        private ICacheProvider _cache;
        private IConfiguration _configuration;

        public PrivilegeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, 
            ILogger<PrivilegeMiddleware> logger, 
            PlatFormDbContext dbContext,
            ICacheProvider cache,
            IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cache = cache;
            _configuration = configuration;
            try
            {
                var result = await this.CheckPrivilege(context);
                if(result) await _next.Invoke(context);
                else
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("You do not have permission to access the requested data or object!");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(e.Message);
            }
        }

        private async Task<bool> CheckPrivilege(HttpContext context)
        {
            try
            {
                UserLastLoginRecord record = await this.GetLoginRecord(context);
                context.Items.Add(nameof(UserLastLoginRecord), record);

                var code = GetFunctionCode(context);
                if (string.IsNullOrEmpty(code)) return true;
                List<string> codes = await GetFunctCodes(record.RoleId);
                return codes.Any(x=>x == code);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        private string GetFunctionCode(HttpContext context)
        {
            string code = string.Empty;
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var myAttribute = endpoint.Metadata.GetMetadata<FunctionAttribute>();
                if (myAttribute != null)
                {
                    code = myAttribute.Code;
                }
            }
            return code;
        }

        private async Task<UserLastLoginRecord> GetLoginRecord(HttpContext context)
        {
            Guid loginId = Guid.Empty;
            var id = context.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(id)) throw new Exception("登录Id为空！");
            else
            {
                Guid.TryParse(id, out loginId);
                if (loginId == Guid.Empty) throw new Exception("登录Id格式化失败！");
            }
            string cacheKey = loginId.ToString();
            UserLastLoginRecord? record = _cache.TryGetValue<UserLastLoginRecord>(cacheKey);
            if (record == null)
                record = await _dbContext.UserLastLoginRecord.FirstOrDefaultAsync(x => x.LoginId == loginId);
            if (record == null) throw new Exception("登录信息为空");
            else
            {
                _cache.Save(cacheKey, record);
                return record;
            }
        }

        private async Task<CtmView> GetCustomer(string ctmId)
        {
            string cacheKey = ctmId;
            CtmView? ctm = _cache.TryGetValue<CtmView>(cacheKey);
            if (ctm == null)
                ctm = await _dbContext.CtmView.FirstOrDefaultAsync(x => x.Id == ctmId);
            if (ctm == null) throw new Exception("客户信息为空");
            else
            {
                _cache.Save(cacheKey, ctm);
                return ctm;
            }
        }

        private async Task<List<string>> GetFunctCodes(string roleId)
        {
            string pgmCode = _configuration["Platform:ProgramCode"];
            string cacheKey = $"{roleId}-{pgmCode}-functs";
            List<string> codes = _cache.TryGetValue<List<string>>(cacheKey);
            if (codes == null)
                codes = await _dbContext.SysRoleFunctView
                    .Where(x=>x.Id == roleId && x.PgmCode == pgmCode)
                    .Select(x=>x.FunctCode)
                    .ToListAsync();
            if (codes == null) codes = new List<string>();
            else _cache.Save(cacheKey, codes);
            return codes;
        }
    }
}
