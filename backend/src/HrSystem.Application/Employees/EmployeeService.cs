using HrSystem.Application.Abstractions;
using HrSystem.Application.Common.Exceptions;
using HrSystem.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace HrSystem.Application.Employees;

public sealed class EmployeeService(IAppDbContext db, ICurrentUser currentUser)
{
    public async Task<EmployeeDto> GetCurrentAsync(CancellationToken cancellationToken)
    {
        var employeeId = currentUser.EmployeeId
            ?? throw new ForbiddenAccessException("The signed-in user is not linked to an employee profile.");

        return await GetByIdAsync(employeeId, cancellationToken);
    }

    public async Task<EmployeeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await db.Employees
                   .AsNoTracking()
                   .Where(e => e.Id == id)
                   .Select(EmployeeDto.Projection)
                   .FirstOrDefaultAsync(cancellationToken)
               ?? throw new NotFoundException(nameof(Employee), id);
    }
}
