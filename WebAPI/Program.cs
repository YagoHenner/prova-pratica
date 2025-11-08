using Application;
using Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<WebApi.Common.HttpResults.ResultSerializer>();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
await app.RunAsync();