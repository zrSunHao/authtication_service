using AutoMapper;
using Hao.Authentication.Domain.Consts;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Manager.Providers;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Manager.Basic
{
    public class BaseManager
    {
        protected readonly PlatFormDbContext _dbContext;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IConfiguration _configuration;
        protected readonly ICacheProvider _cache;
        protected readonly IMyLogProvider _myLog;

        public BaseManager(PlatFormDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ICacheProvider cache, IMyLogProvider myLog)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _myLog = myLog;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetConfiguration(string key) => _configuration[key];

        /// <summary>
        /// 获取当前Http请求头信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected KeyValuePair<string, StringValues> GetHeader(string key) => _httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == key);

        /// <summary>
        /// 生成文件加载url
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string BuilderFileUrl(string? fileName,string key = "")
        {
            if (string.IsNullOrEmpty(fileName)) return "";
            var baseUrl = FileResourceUrl;
            if(string.IsNullOrEmpty(key)) key = CurrentLoginId.ToString();
            if (fileName.Contains(baseUrl)) return fileName;
            return $"{baseUrl}?name={fileName}&key={key}";
        }

        /// <summary>
        /// 机器码
        /// </summary>
        protected string MachineCode => GetConfiguration(CfgConsts.PLATFORM_MACHINE_CODE);

        /// <summary>
        /// 资源地址
        /// </summary>
        protected string FileResourceUrl => GetConfiguration(CfgConsts.FILE_RESOURCE_BASE_URL);

        protected string? RemoteIpAddress => _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        /// <summary>
        /// 用户登录记录
        /// </summary>
        public UserLastLoginRecordM LoginRecord => GetLoginRecord();

        /// <summary>
        /// 当前用户Id
        /// </summary>
        public string CurrentUserId => LoginRecord.CustomerId;
        /// <summary>
        /// 当前用户登录Id
        /// </summary>
        public Guid CurrentLoginId => LoginRecord.LoginId;

        public async Task<SysRoleM> GetCurrentUserRole()
        {
            var record = this.LoginRecord;
            var role = _cache.TryGetValue<SysRoleM>(record.RoleId);
            if(role == null)
            {
                var entity = await _dbContext.SysRoleView.FirstOrDefaultAsync(x => x.Id == record.RoleId);
                if(entity != null)
                {
                    role = _mapper.Map<SysRoleM>(entity);
                    _cache.Save(role.Id, role);
                }
            }
            if (role == null) throw new Exception("获取当前用户的角色信息失败！");
            return role;
        }
        


        private UserLastLoginRecordM? _lastLoginRecord;
        private UserLastLoginRecordM GetLoginRecord()
        {
            if (_lastLoginRecord != null) return _lastLoginRecord;
            _httpContextAccessor.HttpContext.Items.TryGetValue(nameof(UserLastLoginRecordM), out object? obj);
            if (obj == null) throw new MyCustomException("未查询到登录信息！");
            else if (obj is UserLastLoginRecordM) _lastLoginRecord = obj as UserLastLoginRecordM;
            else throw new MyCustomException("未查询到登录信息！");
            if (_lastLoginRecord == null) throw new MyCustomException("未查询到登录信息！");
            else return _lastLoginRecord;
        }
    }
}
