using EU_MES.Domain.Common;
using EU_MES.Domain.Enums;

namespace EU_MES.Domain.Entities;

/// <summary>Represents a machine downtime event.</summary>
public class Downtime : BaseEntity
{
    public Guid MachineId { get; private set; }
    public Guid? WorkOrderId { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public DowntimeCategory Category { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public int? DurationMinutes => EndedAt.HasValue
        ? (int)(EndedAt.Value - StartedAt).TotalMinutes
        : null;
    public Guid? ReportedByWorkerId { get; private set; }

    private Downtime() { }

    public static Downtime Create(Guid machineId, string reason, DowntimeCategory category,
        Guid? workOrderId = null, Guid? reportedByWorkerId = null)
    {
        return new Downtime
        {
            MachineId = machineId,
            Reason = reason,
            Category = category,
            WorkOrderId = workOrderId,
            ReportedByWorkerId = reportedByWorkerId,
            StartedAt = DateTime.UtcNow
        };
    }

    public void End()
    {
        if (EndedAt.HasValue) throw new InvalidOperationException("Downtime already ended.");
        EndedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }
}
