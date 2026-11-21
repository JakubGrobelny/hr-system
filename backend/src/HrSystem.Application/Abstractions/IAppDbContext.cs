using HrSystem.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace HrSystem.Application.Abstractions;

/// <summary>
/// Unit of work over the application's data. Implemented by the EF Core context
/// in the Infrastructure layer; application services depend only on this interface.
/// </summary>
public interface IAppDbContext
{
    DbSet<Employee> Employees { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
