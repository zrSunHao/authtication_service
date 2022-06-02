using AutoMapper;
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
    public class CustomerManager : BaseManager, ICustomerManager
    {
        private readonly ILogger _logger;
        private readonly IConstraintManager _ctt;
        public CustomerManager(PlatFormDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IConstraintManager constraintManager,
            ILogger<ProgramManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
            _ctt = constraintManager;
        }
    }
}
