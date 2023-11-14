using MillerDemo.Api;
using MillerDemo.Api.Jobs;
using MillerDemo.Application;
using MillerDemo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiServices();
app.MapJobsEndpoints();

await app.MigrateDatabaseAsync();

app.Run();