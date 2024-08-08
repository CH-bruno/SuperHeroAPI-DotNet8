using Microsoft.EntityFrameworkCore;
using SuperHeroAPI_DotNet8.Data;
using Microsoft.Extensions.DependencyInjection;
using SuperHeroAPI_DotNet8.Services;
using SuperHeroAPI_DotNet8.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register the ILogWriter services
builder.Services.AddSingleton<ILogWriter, ConsoleLogWriter>();
builder.Services.AddSingleton<ILogWriter>(provider => new FileLogWriter("log.txt"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
