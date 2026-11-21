using HrSystem.Application.Users;
using HrSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HrSystem.Infrastructure.Identity;

public sealed class UserAdministrationService(AppDbContext db, TimeProvider timeProvider) : IUserAdministrationService
{
    public async Task<IReadOnlyList<UserSummaryDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await db.Users
            .AsNoTracking()
            .OrderBy(u => u.Employee.LastName)
            .ThenBy(u => u.Employee.FirstName)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.EmployeeId,
                u.Employee.FirstName,
                u.Employee.LastName,
                u.LockoutEnd,
                Roles = db.Roles
                    .Where(r => db.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == r.Id))
                    .Select(r => r.Name!)
                    .ToList(),
            })
            .ToListAsync(cancellationToken);

        var now = timeProvider.GetUtcNow();

        return users
            .Select(u => new UserSummaryDto(
                u.Id,
                u.Email ?? string.Empty,
                u.EmployeeId,
                $"{u.FirstName} {u.LastName}",
                u.Roles.Order().ToList(),
                u.LockoutEnd > now))
            .ToList();
    }
}
