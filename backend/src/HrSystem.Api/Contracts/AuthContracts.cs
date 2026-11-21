using System.ComponentModel.DataAnnotations;

namespace HrSystem.Api.Contracts;

public sealed record LoginRequest(
    [Required, EmailAddress, MaxLength(256)] string Email,
    [Required, MaxLength(128)] string Password,
    bool RememberMe = false);

public sealed record CurrentUserResponse(
    Guid Id,
    string Email,
    Guid EmployeeId,
    string FirstName,
    string LastName,
    IReadOnlyList<string> Roles);
