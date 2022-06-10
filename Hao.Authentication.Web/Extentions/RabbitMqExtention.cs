using Hao.Authentication.Manager.RabbitMq;

namespace Hao.Authentication.Web.Extentions
{
    public static class RabbitMqExtention
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, ConfigurationManager configuration)
        {
            var serviceClientSettingsConfig = configuration.GetSection("RabbitMq");
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            return services;
        }
    }
}
