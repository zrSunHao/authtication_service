namespace Hao.Authentication.Web.Extentions
{
    public static class CorsExtention
    {
        public static IServiceCollection AddMyCors(this IServiceCollection services, ConfigurationManager configuration)
        {
            var urls = configuration["Platform:Cors"].Split(',');
            services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
            {
                policy.WithOrigins(urls).AllowAnyHeader().WithMethods("POST", " GET", " OPTIONS", " DELETE", " PUT");
            }));
            return services;
        }

        public static IApplicationBuilder UseMyCors(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
            return app;
        }
    }
}
