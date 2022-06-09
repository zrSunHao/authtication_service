using Hao.Authentication.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Manager.Services
{
    public class ConstraintAutoService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConstraintManager _ctt;
        private Timer _timer;

        public ConstraintAutoService(ILogger<ConstraintAutoService> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _ctt = factory.CreateScope().ServiceProvider.GetRequiredService<IConstraintManager>(); ;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(_ctt.AutoUpdateCache, null, 10000, 600000);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }
    }
}
