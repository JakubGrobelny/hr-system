namespace HrSystem.Domain.Common;

/// <summary>
/// Entities implementing this interface get their timestamps maintained automatically
/// by the persistence layer when they are saved.
/// </summary>
public interface IAuditableEntity
{
    DateTimeOffset CreatedAt { get; set; }

    DateTimeOffset? UpdatedAt { get; set; }
}
