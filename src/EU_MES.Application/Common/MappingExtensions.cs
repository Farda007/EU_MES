using EU_MES.Domain.Entities;
using EU_MES.Application.DTOs;
using DomainDowntime = EU_MES.Domain.Entities.Downtime;

namespace EU_MES.Application.Common;

public static class MappingExtensions
{
    public static WorkOrderDto ToDto(this WorkOrder e) => new(
        e.Id, e.OrderNumber, e.ProductCode, e.ProductName,
        e.PlannedQuantity, e.ProducedQuantity, e.ScrapQuantity,
        e.Status, e.Priority, e.PlannedStartDate, e.PlannedEndDate,
        e.ActualStartDate, e.ActualEndDate, e.MachineId, e.CreatedAt, e.UpdatedAt);

    public static OperationDto ToDto(this Operation e) => new(
        e.Id, e.WorkOrderId, e.OperationNumber, e.Name, e.Description,
        e.MachineId, e.WorkerId, e.PlannedDurationMinutes, e.ActualDurationMinutes,
        e.Status, e.StartedAt, e.CompletedAt, e.ProducedQuantity, e.ScrapQuantity);

    public static MachineDto ToDto(this Machine e) => new(
        e.Id, e.Code, e.Name, e.Description, e.WorkCenter, e.Status, e.IsActive, e.CreatedAt);

    public static WorkerDto ToDto(this Worker e) => new(
        e.Id, e.EmployeeNumber, e.FirstName, e.LastName, e.FullName,
        e.Department, e.Shift, e.IsActive);

    public static MaterialDto ToDto(this Material e) => new(
        e.Id, e.Code, e.Name, e.Unit, e.StockQuantity,
        e.ReorderPoint, e.Category, e.IsBelowReorderPoint);

    public static BatchDto ToDto(this Batch e) => new(
        e.Id, e.BatchNumber, e.WorkOrderId, e.MaterialId, e.Quantity,
        e.ProducedAt, e.ExpiryDate, e.Status, e.TraceabilityNotes);

    public static NonConformanceDto ToDto(this NonConformance e) => new(
        e.Id, e.NCNumber, e.WorkOrderId, e.OperationId, e.MachineId,
        e.WorkerId, e.Type, e.Severity, e.Description, e.RootCause,
        e.CorrectiveAction, e.Status, e.DetectedAt, e.ClosedAt);

    public static DowntimeDto ToDto(this DomainDowntime e) => new(
        e.Id, e.MachineId, e.WorkOrderId, e.Reason, e.Category,
        e.StartedAt, e.EndedAt, e.DurationMinutes, e.ReportedByWorkerId);

    public static QualityCheckDto ToDto(this QualityCheck e) => new(
        e.Id, e.WorkOrderId, e.OperationId, e.CheckName, e.CheckType,
        e.Result, e.MeasuredValue, e.MinValue, e.MaxValue,
        e.InspectorWorkerId, e.CheckedAt, e.Notes);
}
