namespace HrSystem.Application.Users;

/// <summary>
/// Administration of user accounts. Accounts are managed by ASP.NET Core Identity,
/// so the implementation lives in the Infrastructure layer.
/// </summary>
public interface IUserAdministrationService
{
    Task<IReadOnlyList<UserSummaryDto>> GetUsersAsync(CancellationToken cancellationToken);
}
