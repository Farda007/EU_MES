using EU_MES.Domain.Common;
using EU_MES.Domain.Enums;

namespace EU_MES.Domain.Entities;

/// <summary>Represents a manufacturing work order.</summary>
public class WorkOrder : BaseEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public string ProductCode { get; private set; } = string.Empty;
    public string ProductName { get; private set; } = string.Empty;
    public decimal PlannedQuantity { get; private set; }
    public decimal ProducedQuantity { get; private set; }
    public decimal ScrapQuantity { get; private set; }
    public WorkOrderStatus Status { get; private set; } = WorkOrderStatus.Created;
    public WorkOrderPriority Priority { get; private set; } = WorkOrderPriority.Normal;
    public DateTime PlannedStartDate { get; private set; }
    public DateTime PlannedEndDate { get; private set; }
    public DateTime? ActualStartDate { get; private set; }
    public DateTime? ActualEndDate { get; private set; }
    public Guid? MachineId { get; private set; }

    private WorkOrder() { }

    /// <summary>Creates a new WorkOrder.</summary>
    public static WorkOrder Create(
        string orderNumber, string productCode, string productName,
        decimal plannedQuantity, WorkOrderPriority priority,
        DateTime plannedStartDate, DateTime plannedEndDate, Guid? machineId = null)
    {
        return new WorkOrder
        {
            OrderNumber = orderNumber,
            ProductCode = productCode,
            ProductName = productName,
            PlannedQuantity = plannedQuantity,
            Priority = priority,
            PlannedStartDate = plannedStartDate,
            PlannedEndDate = plannedEndDate,
            MachineId = machineId
        };
    }

    public void Start()
    {
        if (Status != WorkOrderStatus.Created && Status != WorkOrderStatus.Released)
            throw new InvalidOperationException($"Cannot start work order in status {Status}.");
        Status = WorkOrderStatus.InProgress;
        ActualStartDate = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void Complete()
    {
        if (Status != WorkOrderStatus.InProgress)
            throw new InvalidOperationException("Work order must be InProgress to complete.");
        Status = WorkOrderStatus.Completed;
        ActualEndDate = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void Cancel()
    {
        if (Status == WorkOrderStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed work order.");
        Status = WorkOrderStatus.Cancelled;
        SetUpdatedAt();
    }

    public void Release()
    {
        if (Status != WorkOrderStatus.Created)
            throw new InvalidOperationException("Only created work orders can be released.");
        Status = WorkOrderStatus.Released;
        SetUpdatedAt();
    }

    public void UpdateProducedQuantity(decimal quantity)
    {
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.");
        ProducedQuantity = quantity;
        SetUpdatedAt();
    }

    public void UpdateScrapQuantity(decimal quantity)
    {
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.");
        ScrapQuantity = quantity;
        SetUpdatedAt();
    }

    public void Update(string productCode, string productName, decimal plannedQuantity,
        WorkOrderPriority priority, DateTime plannedStartDate, DateTime plannedEndDate, Guid? machineId)
    {
        ProductCode = productCode;
        ProductName = productName;
        PlannedQuantity = plannedQuantity;
        Priority = priority;
        PlannedStartDate = plannedStartDate;
        PlannedEndDate = plannedEndDate;
        MachineId = machineId;
        SetUpdatedAt();
    }
}
