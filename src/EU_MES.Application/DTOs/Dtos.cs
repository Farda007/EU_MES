using EU_MES.Domain.Enums;

namespace EU_MES.Application.DTOs;

public record WorkOrderDto(
    Guid Id, string OrderNumber, string ProductCode, string ProductName,
    decimal PlannedQuantity, decimal ProducedQuantity, decimal ScrapQuantity,
    WorkOrderStatus Status, WorkOrderPriority Priority,
    DateTime PlannedStartDate, DateTime PlannedEndDate,
    DateTime? ActualStartDate, DateTime? ActualEndDate,
    Guid? MachineId, DateTime CreatedAt, DateTime UpdatedAt);

public record OperationDto(
    Guid Id, Guid WorkOrderId, int OperationNumber, string Name, string Description,
    Guid MachineId, Guid? WorkerId, int PlannedDurationMinutes, int? ActualDurationMinutes,
    OperationStatus Status, DateTime? StartedAt, DateTime? CompletedAt,
    decimal ProducedQuantity, decimal ScrapQuantity);

public record MachineDto(
    Guid Id, string Code, string Name, string Description,
    string WorkCenter, MachineStatus Status, bool IsActive, DateTime CreatedAt);

public record WorkerDto(
    Guid Id, string EmployeeNumber, string FirstName, string LastName,
    string FullName, string Department, WorkerShift Shift, bool IsActive);

public record MaterialDto(
    Guid Id, string Code, string Name, string Unit,
    decimal StockQuantity, decimal ReorderPoint, string Category, bool IsBelowReorderPoint);

public record BatchDto(
    Guid Id, string BatchNumber, Guid WorkOrderId, Guid MaterialId,
    decimal Quantity, DateTime ProducedAt, DateTime? ExpiryDate,
    BatchStatus Status, string TraceabilityNotes);

public record NonConformanceDto(
    Guid Id, string NCNumber, Guid? WorkOrderId, Guid? OperationId,
    Guid? MachineId, Guid? WorkerId, NCType Type, NCSeverity Severity,
    string Description, string? RootCause, string? CorrectiveAction,
    NCStatus Status, DateTime DetectedAt, DateTime? ClosedAt);

public record DowntimeDto(
    Guid Id, Guid MachineId, Guid? WorkOrderId, string Reason,
    DowntimeCategory Category, DateTime StartedAt, DateTime? EndedAt,
    int? DurationMinutes, Guid? ReportedByWorkerId);

public record QualityCheckDto(
    Guid Id, Guid WorkOrderId, Guid? OperationId, string CheckName,
    QualityCheckType CheckType, QualityCheckResult Result,
    decimal? MeasuredValue, decimal? MinValue, decimal? MaxValue,
    Guid? InspectorWorkerId, DateTime CheckedAt, string? Notes);

public record OEEReportDto(
    Guid MachineId, string MachineCode, string MachineName,
    DateTime From, DateTime To,
    double PlannedTimeMinutes, double DowntimeMinutes,
    double Availability, double Performance, double Quality, double OEE,
    int TotalProduced, int TotalGood, int TotalScrap);

public record DashboardSummaryDto(
    int TotalWorkOrders, int ActiveWorkOrders, int CompletedWorkOrders,
    int TotalMachines, int RunningMachines, int MachinesInBreakdown,
    int OpenNonConformances, int CriticalNCs,
    double OverallOEE, int TodayProduction);
