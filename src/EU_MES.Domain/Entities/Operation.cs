using EU_MES.Domain.Common;
using EU_MES.Domain.Enums;

namespace EU_MES.Domain.Entities;

/// <summary>Represents an operation within a work order.</summary>
public class Operation : BaseEntity
{
    public Guid WorkOrderId { get; private set; }
    public int OperationNumber { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid MachineId { get; private set; }
    public Guid? WorkerId { get; private set; }
    public int PlannedDurationMinutes { get; private set; }
    public int? ActualDurationMinutes { get; private set; }
    public OperationStatus Status { get; private set; } = OperationStatus.Pending;
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public decimal ProducedQuantity { get; private set; }
    public decimal ScrapQuantity { get; private set; }

    private Operation() { }

    public static Operation Create(Guid workOrderId, int operationNumber, string name,
        string description, Guid machineId, int plannedDurationMinutes, Guid? workerId = null)
    {
        return new Operation
        {
            WorkOrderId = workOrderId,
            OperationNumber = operationNumber,
            Name = name,
            Description = description,
            MachineId = machineId,
            PlannedDurationMinutes = plannedDurationMinutes,
            WorkerId = workerId
        };
    }

    public void Start()
    {
        if (Status != OperationStatus.Pending && Status != OperationStatus.Paused)
            throw new InvalidOperationException($"Cannot start operation in status {Status}.");
        Status = OperationStatus.InProgress;
        StartedAt ??= DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void Pause()
    {
        if (Status != OperationStatus.InProgress)
            throw new InvalidOperationException("Only InProgress operations can be paused.");
        Status = OperationStatus.Paused;
        SetUpdatedAt();
    }

    public void Complete(decimal producedQty, decimal scrapQty)
    {
        if (Status != OperationStatus.InProgress)
            throw new InvalidOperationException("Only InProgress operations can be completed.");
        Status = OperationStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        ProducedQuantity = producedQty;
        ScrapQuantity = scrapQty;
        if (StartedAt.HasValue)
            ActualDurationMinutes = (int)(DateTime.UtcNow - StartedAt.Value).TotalMinutes;
        SetUpdatedAt();
    }

    public void AssignWorker(Guid workerId)
    {
        WorkerId = workerId;
        SetUpdatedAt();
    }
}
