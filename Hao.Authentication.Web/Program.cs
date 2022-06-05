using Hao.Authentication.Manager;
using Hao.Authentication.Persistence.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Text;

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
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Please enter into field the word 'Bearer ' followed by a space and the JWT value",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, Array.Empty<string>() }
                });
});

var tokenKey = builder.Configuration["Platform:Key"];
var issuer = builder.Configuration["Platform:Issuer"];
builder.Services
    .AddAuthentication(op =>
    {
        op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            //ClockSkew = TimeSpan.Zero
        };
    });

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
    try
    {
        await next.Invoke();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    // Do logging or other work that doesn't write to the Response.
    watch.Stop();
    Console.WriteLine($"本次请求耗时{watch.ElapsedMilliseconds}毫秒");
});

app.UseHttpsRedirection();
app.UseAuthentication();
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
