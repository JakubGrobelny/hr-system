using System.Security.Claims;
using HrSystem.Application.Abstractions;
using HrSystem.Application.Common.Security;

namespace HrSystem.Api.Security;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

    public Guid? UserId => GetGuidClaim(ClaimTypes.NameIdentifier);

    public Guid? EmployeeId => GetGuidClaim(AppClaimTypes.EmployeeId);

    public bool IsInRole(string role) => Principal?.IsInRole(role) == true;

    private Guid? GetGuidClaim(string claimType) =>
        Guid.TryParse(Principal?.FindFirstValue(claimType), out var value) ? value : null;
}
