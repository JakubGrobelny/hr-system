using HrSystem.Application.Common.Security;
using HrSystem.Domain.Employees;
using HrSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HrSystem.Infrastructure.Persistence.Seeding;

/// <summary>
/// Applies pending migrations and seeds the data the application cannot work without:
/// roles, the initial administrator and (optionally) demo accounts.
/// </summary>
public sealed class DatabaseInitializer(
    AppDbContext db,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IOptions<SeedOptions> options,
    ILogger<DatabaseInitializer> logger)
{
    private sealed record SeedUser(string Email, string FirstName, string LastName, string[] Roles);

    private static readonly SeedUser[] DemoUsers =
    [
        new("hr@hr.local", "Anna", "Kowalska", [Roles.Employee, Roles.Hr]),
        new("manager@hr.local", "Piotr", "Nowak", [Roles.Employee, Roles.Manager]),
        new("employee@hr.local", "Jan", "Wiśniewski", [Roles.Employee]),
    ];

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (db.Database.IsRelational())
        {
            await db.Database.MigrateAsync(cancellationToken);
        }
        else
        {
            await db.Database.EnsureCreatedAsync(cancellationToken);
        }

        await SeedRolesAsync();
        await SeedAdministratorAsync();

        if (options.Value.DemoUsers)
        {
            await SeedDemoUsersAsync();
        }
    }

    private async Task SeedRolesAsync()
    {
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                EnsureSucceeded(await roleManager.CreateAsync(new IdentityRole<Guid>(role)), $"create role '{role}'");
            }
        }
    }

    private async Task SeedAdministratorAsync()
    {
        var seed = options.Value;
        if (string.IsNullOrWhiteSpace(seed.AdminPassword))
        {
            logger.LogWarning("Seed:AdminPassword is not configured - the administrator account will not be created.");
            return;
        }

        await EnsureUserAsync(
            new SeedUser(seed.AdminEmail, "Administrator", "Systemu", [Roles.Employee, Roles.Admin]),
            seed.AdminPassword);
    }

    private async Task SeedDemoUsersAsync()
    {
        var password = options.Value.DemoUsersPassword;
        if (string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Seed:DemoUsersPassword is not configured - demo accounts will not be created.");
            return;
        }

        foreach (var demoUser in DemoUsers)
        {
            await EnsureUserAsync(demoUser, password);
        }
    }

    private async Task EnsureUserAsync(SeedUser seed, string password)
    {
        if (await userManager.FindByEmailAsync(seed.Email) is not null)
        {
            return;
        }

        var employee = new Employee(seed.FirstName, seed.LastName, seed.Email);
        var user = new ApplicationUser
        {
            UserName = employee.Email,
            Email = employee.Email,
            EmailConfirmed = true,
            Employee = employee,
            EmployeeId = employee.Id,
        };

        EnsureSucceeded(await userManager.CreateAsync(user, password), $"create user '{seed.Email}'");
        EnsureSucceeded(await userManager.AddToRolesAsync(user, seed.Roles), $"assign roles to '{seed.Email}'");

        logger.LogInformation("Seeded user {Email} with roles {Roles}", seed.Email, string.Join(", ", seed.Roles));
    }

    private static void EnsureSucceeded(IdentityResult result, string operation)
    {
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Database seeding failed to {operation}: {errors}");
        }
    }
}
