using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;

namespace EU_MES.Domain.Interfaces;

public interface IWorkOrderRepository : IRepository<WorkOrder>
{
    Task<WorkOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default);
    Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken ct = default);
    Task<IEnumerable<WorkOrder>> GetByFiltersAsync(WorkOrderStatus? status, WorkOrderPriority? priority,
        DateTime? from, DateTime? to, CancellationToken ct = default);
}

public interface IOperationRepository : IRepository<Operation>
{
    Task<IEnumerable<Operation>> GetByWorkOrderIdAsync(Guid workOrderId, CancellationToken ct = default);
    Task<IEnumerable<Operation>> GetByMachineIdAsync(Guid machineId, CancellationToken ct = default);
}

public interface IMachineRepository : IRepository<Machine>
{
    Task<Machine?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IEnumerable<Machine>> GetActiveAsync(CancellationToken ct = default);
}

public interface IWorkerRepository : IRepository<Worker>
{
    Task<Worker?> GetByEmployeeNumberAsync(string empNumber, CancellationToken ct = default);
    Task<IEnumerable<Worker>> GetActiveAsync(CancellationToken ct = default);
}

public interface IMaterialRepository : IRepository<Material>
{
    Task<Material?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IEnumerable<Material>> GetBelowReorderPointAsync(CancellationToken ct = default);
}

public interface IBatchRepository : IRepository<Batch>
{
    Task<IEnumerable<Batch>> GetByWorkOrderIdAsync(Guid workOrderId, CancellationToken ct = default);
}

public interface IDowntimeRepository : IRepository<Downtime>
{
    Task<IEnumerable<Downtime>> GetByMachineIdAsync(Guid machineId, CancellationToken ct = default);
    Task<IEnumerable<Downtime>> GetActiveDowntimesAsync(CancellationToken ct = default);
    Task<IEnumerable<Downtime>> GetByDateRangeAsync(Guid machineId, DateTime from, DateTime to, CancellationToken ct = default);
}

public interface INonConformanceRepository : IRepository<NonConformance>
{
    Task<IEnumerable<NonConformance>> GetByWorkOrderIdAsync(Guid workOrderId, CancellationToken ct = default);
    Task<IEnumerable<NonConformance>> GetOpenAsync(CancellationToken ct = default);
    Task<string> GenerateNCNumberAsync(CancellationToken ct = default);
}

public interface IQualityCheckRepository : IRepository<QualityCheck>
{
    Task<IEnumerable<QualityCheck>> GetByWorkOrderIdAsync(Guid workOrderId, CancellationToken ct = default);
    Task<IEnumerable<QualityCheck>> GetByOperationIdAsync(Guid operationId, CancellationToken ct = default);
}
