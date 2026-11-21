using System.Security.Claims;
using HrSystem.Application.Common.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HrSystem.Infrastructure.Identity;

/// <summary>
/// Adds the linked employee id to the authentication cookie, so that application services
/// can resolve the current employee without an extra database lookup.
/// </summary>
public sealed class AppUserClaimsPrincipalFactory(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IOptions<IdentityOptions> options)
    : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<Guid>>(userManager, roleManager, options)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim(AppClaimTypes.EmployeeId, user.EmployeeId.ToString()));
        return identity;
    }
}
