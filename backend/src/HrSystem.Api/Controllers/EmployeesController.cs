using HrSystem.Application.Employees;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Api.Controllers;

[ApiController]
[Route("api/employees")]
public sealed class EmployeesController(EmployeeService employees) : ControllerBase
{
    /// <summary>Profile of the signed-in employee.</summary>
    [HttpGet("me")]
    [ProducesResponseType<EmployeeDto>(StatusCodes.Status200OK)]
    public Task<EmployeeDto> GetMe(CancellationToken cancellationToken) =>
        employees.GetCurrentAsync(cancellationToken);

    /// <summary>Basic profile of any employee (visible to every signed-in user).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<EmployeeDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<EmployeeDto> GetById(Guid id, CancellationToken cancellationToken) =>
        employees.GetByIdAsync(id, cancellationToken);
}
