using HrSystem.Api.Security;
using HrSystem.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Api.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Policy = Policies.Admin)]
public sealed class AdminUsersController(IUserAdministrationService users) : ControllerBase
{
    /// <summary>All user accounts with their roles.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<UserSummaryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<IReadOnlyList<UserSummaryDto>> GetAll(CancellationToken cancellationToken) =>
        users.GetUsersAsync(cancellationToken);
}
