using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Manager.Implements;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hao.Authentication.Manager
{
    public static class DomainInjection
    {
        public static IServiceCollection DomainConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DomainInjection).Assembly);

            services.AddTransient<IProgramManager, ProgramManager>();
            return services;
        }

        //public static IApplicationBuilder DomainConfigure(this IApplicationBuilder app)
        //{
        //    return app;
        //}
    }
}
