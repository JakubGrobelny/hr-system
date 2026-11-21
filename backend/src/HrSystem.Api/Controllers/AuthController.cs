using System.Security.Claims;
using HrSystem.Api.Contracts;
using HrSystem.Application.Common.Security;
using HrSystem.Application.Employees;
using HrSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    EmployeeService employees) : ControllerBase
{
    /// <summary>Signs the user in and issues the authentication cookie.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType<CurrentUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CurrentUserResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            return InvalidCredentials();
        }

        var result = await signInManager.PasswordSignInAsync(
            user, request.Password, request.RememberMe, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Account locked",
                detail: "Too many failed sign-in attempts. Try again later.");
        }

        if (!result.Succeeded)
        {
            return InvalidCredentials();
        }

        var principal = await signInManager.CreateUserPrincipalAsync(user);
        return await ToResponseAsync(principal, cancellationToken);
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return NoContent();
    }

    /// <summary>Returns the signed-in user together with their roles.</summary>
    [HttpGet("me")]
    [ProducesResponseType<CurrentUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CurrentUserResponse>> Me(CancellationToken cancellationToken) =>
        await ToResponseAsync(User, cancellationToken);

    private async Task<CurrentUserResponse> ToResponseAsync(ClaimsPrincipal principal, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var employeeId = Guid.Parse(principal.FindFirstValue(AppClaimTypes.EmployeeId)!);
        var employee = await employees.GetByIdAsync(employeeId, cancellationToken);
        var roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).Order().ToList();

        return new CurrentUserResponse(userId, employee.Email, employee.Id, employee.FirstName, employee.LastName, roles);
    }

    private ObjectResult InvalidCredentials() =>
        Problem(
            statusCode: StatusCodes.Status401Unauthorized,
            title: "Invalid credentials",
            detail: "Invalid e-mail or password.");
}
