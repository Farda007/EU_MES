namespace EU_MES.Domain.Common;

/// <summary>Base class for all domain entities.</summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;

    public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
}
