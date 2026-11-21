namespace HrSystem.Application.Abstractions;

/// <summary>
/// Information about the user executing the current request.
/// </summary>
public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    Guid? UserId { get; }

    Guid? EmployeeId { get; }

    bool IsInRole(string role);
}
