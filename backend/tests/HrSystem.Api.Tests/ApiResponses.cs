namespace HrSystem.Api.Tests;

// Minimal copies of API response shapes, kept independent from the production types
// so that the tests detect accidental changes in the public contract.
internal sealed record CurrentUser(Guid Id, string Email, Guid EmployeeId, string FirstName, string LastName, string[] Roles);

internal sealed record Employee(Guid Id, string FirstName, string LastName, string Email, bool IsActive);

internal sealed record UserSummary(Guid Id, string Email, Guid EmployeeId, string FullName, string[] Roles, bool IsLockedOut);

internal sealed record Problem(string? Title, int? Status, string? Detail);
