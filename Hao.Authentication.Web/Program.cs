using Hao.Authentication.Persistence.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

builder.Services.AddDbContext<PlatFormDbContext>(
                config => config.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
                optionBuilder => optionBuilder.MigrationsAssembly(typeof(PlatFormDbContext).Assembly.GetName().Name))
                );

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
