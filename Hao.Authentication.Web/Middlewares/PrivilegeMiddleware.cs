using Hao.Authentication.Common.Enums;
using Hao.Authentication.Domain.Consts;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Manager.Providers;
using Hao.Authentication.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Hao.Authentication.Web.Middlewares
{
    public class PrivilegeMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<PrivilegeMiddleware> _logger;
        private IConfiguration _configuration;

        public PrivilegeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
            ILogger<PrivilegeMiddleware> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            try
            {
                var result = await this.CheckPrivilege(context);
                if (result) await _next.Invoke(context);
                else
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("You do not have permission to access the requested data or object!");
                }
            }
            catch(MyUnauthorizedException e)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(e.Message);
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
                var allow = this.AllowAnonymous(context);
                if (allow) return true;

                IPrivilegeManager? manager = context.RequestServices.GetService<IPrivilegeManager>();
                if (manager == null) throw new Exception("privilege manager not instance!");
                UserLastLoginRecordM record = await this.GetLoginRecord(context, manager);
                context.Items.Add(nameof(UserLastLoginRecordM), record);

                var p = _configuration[CfgConsts.PLATFORM_PERMISSION];
                if (!string.IsNullOrEmpty(p))
                {
                    var pa = CfgConsts.PLATFORM_PERMISSION_ALLOW;
                    if (p == pa) return true;
                }

                var role = await manager.GetRoleById(record.RoleId);
                if(role.Rank >= SysRoleRank.sys_manager) return true;

                var code = GetFunctionCode(context);
                if (string.IsNullOrEmpty(code)) return true;
                List<string> codes = await manager.GetFunctCodes(record.RoleId);
                return codes.Any(x => x == code);
            }
            catch(MyUnauthorizedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        private bool AllowAnonymous(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var myAttribute = endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>();
                if (myAttribute != null) return true;
            }
            return false;
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

        private async Task<UserLastLoginRecordM> GetLoginRecord(HttpContext context, IPrivilegeManager manager)
        {
            Guid loginId = Guid.Empty;
            var sid = context.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;
            return await manager.GetLoginRecord(sid);
        }
    }
}
