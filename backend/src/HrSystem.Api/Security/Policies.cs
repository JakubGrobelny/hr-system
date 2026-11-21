namespace HrSystem.Api.Security;

/// <summary>
/// Authorization policy names used with <c>[Authorize(Policy = ...)]</c>.
/// Data-level rules (e.g. "a manager sees only their subordinates") are enforced in application services.
/// </summary>
public static class Policies
{
    public const string Manager = "RequireManager";
    public const string Hr = "RequireHr";
    public const string Admin = "RequireAdmin";
}
