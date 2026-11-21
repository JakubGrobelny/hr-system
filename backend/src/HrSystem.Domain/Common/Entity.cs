namespace HrSystem.Domain.Common;

/// <summary>
/// Base class for all domain entities. Identifiers are time-ordered GUIDs (v7),
/// which keeps B-tree indexes in PostgreSQL compact while allowing ids to be
/// generated in the application before the entity is persisted.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; protected init; } = Guid.CreateVersion7();
}
