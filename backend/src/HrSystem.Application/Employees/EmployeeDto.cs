using System.Linq.Expressions;
using HrSystem.Domain.Employees;

namespace HrSystem.Application.Employees;

/// <summary>
/// Basic employee profile, visible to every authenticated user.
/// </summary>
public sealed record EmployeeDto(Guid Id, string FirstName, string LastName, string Email, bool IsActive)
{
    public static readonly Expression<Func<Employee, EmployeeDto>> Projection =
        e => new EmployeeDto(e.Id, e.FirstName, e.LastName, e.Email, e.IsActive);
}
