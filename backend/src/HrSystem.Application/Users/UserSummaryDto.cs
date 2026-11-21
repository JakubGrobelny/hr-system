namespace HrSystem.Application.Users;

public sealed record UserSummaryDto(
    Guid Id,
    string Email,
    Guid EmployeeId,
    string FullName,
    IReadOnlyList<string> Roles,
    bool IsLockedOut);
