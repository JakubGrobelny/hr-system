using HrSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HrSystem.Api.Tests.Infrastructure;

/// <summary>
/// Hosts the real API in memory. PostgreSQL is replaced with the EF Core in-memory provider,
/// so the tests exercise routing, authentication, authorization and serialization without
/// external dependencies. Database-specific behaviour is covered separately against PostgreSQL.
/// </summary>
public sealed class HrSystemApiFactory : WebApplicationFactory<Program>
{
    public const string AdminEmail = "admin@hr.local";
    public const string EmployeeEmail = "employee@hr.local";
    public const string Password = "Test123!";

    private readonly string _databaseName = $"hr-system-tests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("Seed:AdminEmail", AdminEmail);
        builder.UseSetting("Seed:AdminPassword", Password);
        builder.UseSetting("Seed:DemoUsers", "true");
        builder.UseSetting("Seed:DemoUsersPassword", Password);

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(_databaseName));
        });
    }
}
