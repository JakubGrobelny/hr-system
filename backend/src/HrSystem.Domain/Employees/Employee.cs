using HrSystem.Domain.Common;

namespace HrSystem.Domain.Employees;

/// <summary>
/// Employee profile. Every user account is linked to exactly one employee.
/// Organizational data (team, position, manager) and employment details are added
/// by the Organization and Employees (HR) modules.
/// </summary>
public sealed class Employee : Entity, IAuditableEntity
{
    public const int NameMaxLength = 100;
    public const int EmailMaxLength = 256;

    // Required by EF Core.
    private Employee()
    {
        FirstName = null!;
        LastName = null!;
        Email = null!;
    }

    public Employee(string firstName, string lastName, string email)
    {
        FirstName = ValidateName(firstName, nameof(firstName));
        LastName = ValidateName(lastName, nameof(lastName));
        Email = NormalizeEmail(email);
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public void Rename(string firstName, string lastName)
    {
        FirstName = ValidateName(firstName, nameof(firstName));
        LastName = ValidateName(lastName, nameof(lastName));
    }

    public void Deactivate() => IsActive = false;

    private static string ValidateName(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"Field '{paramName}' is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException($"Field '{paramName}' cannot exceed {NameMaxLength} characters.");
        }

        return trimmed;
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new DomainException("A valid e-mail address is required.");
        }

        var normalized = email.Trim().ToLowerInvariant();
        if (normalized.Length > EmailMaxLength)
        {
            throw new DomainException($"E-mail cannot exceed {EmailMaxLength} characters.");
        }

        return normalized;
    }
}
