using Hao.Authentication.Manager;
using Hao.Authentication.Web.Extentions;
using Hao.Authentication.Web.Middlewares;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddMyCors(builder.Configuration);
builder.Services.AddHttpContextAccessor();
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

app.UseMiddleware<MonitorMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseMyCors();
app.UseAuthorization();
app.UseMiddleware<PrivilegeMiddleware>();
app.MapControllers();

#endregion

app.Run();
