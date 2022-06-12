using Hao.Authentication.Manager;
using Hao.Authentication.Web.Extentions;
using Hao.Authentication.Web.Middlewares;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            //.WriteTo.File("logs\\log_.txt", LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
            .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console());

    #region Add services to the container.
    builder.Services.AddDbContext(builder.Configuration);
    builder.Services.AddMyCors(builder.Configuration);
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddRabbitMq(builder.Configuration);
    builder.Services.DomainConfigureServices();
    builder.Services.AddControllers();
    builder.Services.AddSwagger();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    #endregion

    var app = builder.Build();
    #region  Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseRouting();
    app.UseMyCors();
    app.UseAuthorization();
    app.UseMiddleware<PrivilegeMiddleware>();
    app.UseMiddleware<MonitorMiddleware>();
    app.MapControllers();
    #endregion

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


