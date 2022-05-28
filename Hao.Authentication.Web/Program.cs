using Hao.Authentication.Manager;
using Hao.Authentication.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

builder.Services.AddDbContext<PlatFormDbContext>(
                config => config.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
                optionBuilder => optionBuilder.MigrationsAssembly(typeof(PlatFormDbContext).Assembly.GetName().Name))
                );

var urls = builder.Configuration["Platform:Cors"].Split(',');
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
{
    policy.WithOrigins("http://localhost:4200/", "http://127.0.0.1:4200/").AllowAnyHeader().WithMethods("POST", " GET", " OPTIONS", " DELETE", " PUT");
}));
builder.Services.AddHttpContextAccessor();
builder.Services.DomainConfigureServices();
builder.Services.AddControllers();

// Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region  Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    Stopwatch watch = new Stopwatch();
    watch.Start();
    // Do work that can write to the Response.
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
    watch.Stop();
    Console.WriteLine($"本次请求耗时{watch.ElapsedMilliseconds}毫秒");
});

app.UseHttpsRedirection();

app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Origin-Headers", "*");
        context.Response.StatusCode = 204;

        return;
    }
    // Do work that can write to the Response.
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});
app.UseCors("CorsPolicy");


app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
