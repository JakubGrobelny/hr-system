using HrSystem.Domain.Employees;
using Microsoft.AspNetCore.Identity;

namespace HrSystem.Infrastructure.Identity;

/// <summary>
/// User account (credentials, roles, lockout) managed by ASP.NET Core Identity.
/// Each account is linked to exactly one <see cref="Domain.Employees.Employee"/> profile.
/// </summary>
public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        Id = Guid.CreateVersion7();
        SecurityStamp = Guid.NewGuid().ToString();
    }

    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
}
