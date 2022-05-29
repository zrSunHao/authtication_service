﻿using AutoMapper;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Persistence.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Manager.Implements
{
    public class ConstraintManager : BaseManager, IConstraintManager
    {
        private readonly ILogger _logger;
        public ConstraintManager(PlatFormDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ConstraintManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }
    }
}