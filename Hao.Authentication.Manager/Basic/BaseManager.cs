﻿using AutoMapper;
using Hao.Authentication.Persistence.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 机器码
        /// </summary>
        protected string MachineCode => GetConfiguration("Platform:MachineCode");

        /// <summary>
        /// 当前用户Id
        /// </summary>
        public string CurrentUserId = "Ctm20220527171601001";
    }
}
