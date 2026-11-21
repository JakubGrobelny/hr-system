using HrSystem.Api;
using HrSystem.Application;
using HrSystem.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference().AllowAnonymous();
}
else
{
    // TLS is terminated by the reverse proxy (see frontend/nginx.conf); HSTS is sent for HTTPS requests.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

await app.Services.InitializeDatabaseAsync();

await app.RunAsync();

// Makes the entry point visible to WebApplicationFactory in integration tests.
public partial class Program;
