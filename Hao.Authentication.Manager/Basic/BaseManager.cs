using AutoMapper;
using Hao.Authentication.Persistence.Database;
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
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IConfiguration _configuration;

        public BaseManager(PlatFormDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = _httpContextAccessor.HttpContext.RequestServices;
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
        protected string MachineCode => GetConfiguration("Platform:MachineCode");

        /// <summary>
        /// 资源地址
        /// </summary>
        protected string FileResourceUrl => GetConfiguration("FileResourceBaseUrl");

        /// <summary>
        /// 当前用户Id
        /// </summary>
        public string CurrentUserId => GetCurrentUserId();
        /// <summary>
        /// 当前用户登录Id
        /// </summary>
        public Guid CurrentLoginId => this.GetUserLoginId();

        private string GetCurrentUserId()
        {
            Guid loginId = GetUserLoginId();
            var record = _dbContext.UserLastLoginRecord
                .FirstOrDefault(x => x.LoginId == loginId);
            if (record == null) throw new MyCustomException("未查询到登录信息！");
            return record.CustomerId;
        }

        private Guid GetUserLoginId() 
        { 
            var id = _httpContextAccessor.HttpContext
                .User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(id)) throw new MyCustomException("未查询到登录信息！");
            Guid.TryParse(id, out Guid loginId);
            if(loginId== Guid.Empty) throw new MyCustomException("未查询到登录信息！");
            return loginId;
        }
    }
}
