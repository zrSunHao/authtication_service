using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Manager.Implements;
using Hao.Authentication.Manager.Providers;
using Hao.Authentication.Manager.RabbitMq;
using Hao.Authentication.Manager.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hao.Authentication.Manager
{
    public static class DomainInjection
    {
        public static IServiceCollection DomainConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DomainInjection).Assembly);
            services.AddMemoryCache();
            services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
            services.AddSingleton<IRabbitMqDirectSender, RabbitMqDirectSender>();

            services.AddTransient<IMyLogProvider, MyLogProvider>();

            services.AddTransient<IConstraintManager, ConstraintManager>();
            services.AddTransient<IPrivilegeManager, PrivilegeManager>();

            services.AddTransient<IProgramManager, ProgramManager>();
            services.AddTransient<IResourceManager, ResourceManager>();
            services.AddTransient<ISysManager, SysManager>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<ICustomerManager, CustomerManager>();
            services.AddTransient<IReportManager, ReportManager>();

            services.AddHostedService<ConstraintAutoService>();
            services.AddHostedService<RabbitMqDirectReceiver>();

            return services;
        }

        //public static IApplicationBuilder DomainConfigure(this IApplicationBuilder app)
        //{
        //    return app;
        //}
    }
}
