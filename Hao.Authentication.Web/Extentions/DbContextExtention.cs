using Hao.Authentication.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Hao.Authentication.Web.Extentions
{
    public static class DbContextExtention
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, ConfigurationManager configuration)
        {
            string connectStr = configuration.GetConnectionString("Default");
            services.AddDbContext<PlatFormDbContext>(
                config => config.UseSqlServer(connectStr,
                optionBuilder => optionBuilder.MigrationsAssembly(typeof(PlatFormDbContext).Assembly.GetName().Name))
                );
            return services;
        }
    }
}
