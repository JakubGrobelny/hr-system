namespace HrSystem.Infrastructure.Persistence.Seeding;

public sealed class SeedOptions
{
    public const string SectionName = "Seed";

    /// <summary>E-mail of the initial administrator account.</summary>
    public string AdminEmail { get; set; } = "admin@hr.local";

    /// <summary>Password of the initial administrator. When empty, the account is not created.</summary>
    public string? AdminPassword { get; set; }

    /// <summary>Creates sample accounts for every role. Intended for development only.</summary>
    public bool DemoUsers { get; set; }

    public string? DemoUsersPassword { get; set; }
}
