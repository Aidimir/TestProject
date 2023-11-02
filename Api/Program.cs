using Dal.Repositories;
using Dal.Exceptions;
using Api.MIddlewares;
using Logic.Features;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Api.DepencyRegistration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Games_api",
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var fullpath = $"{AppDomain.CurrentDomain.BaseDirectory + xmlFilename}";
    options.IncludeXmlComments(fullpath); ;
});

builder.Services.AddLogicServices();
builder.Services.AddRepositories(builder.Configuration.GetConnectionString("WebApiDatabase"));

var app = builder.Build();

// Разрешаю сваггер не в дебаге специально для вас.
app.UseSwagger(option => option.SerializeAsV2 = true);
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();

