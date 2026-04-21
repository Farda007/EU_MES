using EU_MES.Domain.Common;
using EU_MES.Domain.Enums;

namespace EU_MES.Domain.Entities;

/// <summary>Represents a non-conformance report.</summary>
public class NonConformance : BaseEntity
{
    public string NCNumber { get; private set; } = string.Empty;
    public Guid? WorkOrderId { get; private set; }
    public Guid? OperationId { get; private set; }
    public Guid? MachineId { get; private set; }
    public Guid? WorkerId { get; private set; }
    public NCType Type { get; private set; }
    public NCSeverity Severity { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string? RootCause { get; private set; }
    public string? CorrectiveAction { get; private set; }
    public NCStatus Status { get; private set; } = NCStatus.Open;
    public DateTime DetectedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }

    private NonConformance() { }

    public static NonConformance Create(string ncNumber, NCType type, NCSeverity severity,
        string description, Guid? workOrderId = null, Guid? operationId = null,
        Guid? machineId = null, Guid? workerId = null)
    {
        return new NonConformance
        {
            NCNumber = ncNumber,
            Type = type,
            Severity = severity,
            Description = description,
            WorkOrderId = workOrderId,
            OperationId = operationId,
            MachineId = machineId,
            WorkerId = workerId,
            DetectedAt = DateTime.UtcNow
        };
    }

    public void SetUnderReview() { Status = NCStatus.UnderReview; SetUpdatedAt(); }
    public void RequireAction() { Status = NCStatus.ActionRequired; SetUpdatedAt(); }

    public void Close(string rootCause, string correctiveAction)
    {
        Status = NCStatus.Closed;
        RootCause = rootCause;
        CorrectiveAction = correctiveAction;
        ClosedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }
}
