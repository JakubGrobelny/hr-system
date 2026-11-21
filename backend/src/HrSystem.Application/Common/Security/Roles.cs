namespace HrSystem.Application.Common.Security;

/// <summary>
/// Roles recognized by the system. A user can have several roles at once
/// (e.g. a manager is also an employee).
/// </summary>
public static class Roles
{
    public const string Employee = "Employee";
    public const string Manager = "Manager";
    public const string Hr = "Hr";
    public const string Admin = "Admin";

    public static readonly IReadOnlyList<string> All = [Employee, Manager, Hr, Admin];
}
