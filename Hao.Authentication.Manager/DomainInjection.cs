using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Manager.Implements;
using Hao.Authentication.Manager.Providers;
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

            services.AddTransient<IProgramManager, ProgramManager>();
            services.AddTransient<IConstraintManager, ConstraintManager>();
            services.AddTransient<IResourceManager, ResourceManager>();
            services.AddTransient<ISysManager, SysManager>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<ICustomerManager, CustomerManager>();
            services.AddTransient<IReportManager, ReportManager>();

            return services;
        }

        //public static IApplicationBuilder DomainConfigure(this IApplicationBuilder app)
        //{
        //    return app;
        //}
    }
}
