using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using EU_MES.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EU_MES.Infrastructure.Persistence.Repositories;

public class WorkOrderRepository : BaseRepository<WorkOrder>, IWorkOrderRepository
{
    public WorkOrderRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<WorkOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(w => w.OrderNumber == orderNumber, ct);

    public async Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken ct = default)
        => await _dbSet.Where(w => w.Status == status).ToListAsync(ct);

    public async Task<IEnumerable<WorkOrder>> GetByFiltersAsync(
        WorkOrderStatus? status, WorkOrderPriority? priority,
        DateTime? from, DateTime? to, CancellationToken ct = default)
    {
        var query = _dbSet.AsQueryable();
        if (status.HasValue) query = query.Where(w => w.Status == status.Value);
        if (priority.HasValue) query = query.Where(w => w.Priority == priority.Value);
        if (from.HasValue) query = query.Where(w => w.PlannedStartDate >= from.Value);
        if (to.HasValue) query = query.Where(w => w.PlannedEndDate <= to.Value);
        return await query.OrderByDescending(w => w.CreatedAt).ToListAsync(ct);
    }
}

public class OperationRepository : BaseRepository<Operation>, IOperationRepository
{
    public OperationRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<Operation>> GetByWorkOrderIdAsync(Guid workOrderId, CancellationToken ct = default)
        => await _dbSet.Where(o => o.WorkOrderId == workOrderId).OrderBy(o => o.OperationNumber).ToListAsync(ct);

    public async Task<IEnumerable<Operation>> GetByMachineIdAsync(Guid machineId, CancellationToken ct = default)
        => await _dbSet.Where(o => o.MachineId == machineId).ToListAsync(ct);
}

public class MachineRepository : BaseRepository<Machine>, IMachineRepository
{
    public MachineRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Machine?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(m => m.Code == code, ct);

    public async Task<IEnumerable<Machine>> GetActiveAsync(CancellationToken ct = default)
        => await _dbSet.Where(m => m.IsActive).ToListAsync(ct);
}

public class WorkerRepository : BaseRepository<Worker>, IWorkerRepository
{
    public WorkerRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Worker?> GetByEmployeeNumberAsync(string empNumber, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(w => w.EmployeeNumber == empNumber, ct);

    public async Task<IEnumerable<Worker>> GetActiveAsync(CancellationToken ct = default)
        => await _dbSet.Where(w => w.IsActive).ToListAsync(ct);
}

public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
{
    public MaterialRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Material?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(m => m.Code == code, ct);

    public async Task<IEnumerable<Material>> GetBelowReorderPointAsync(CancellationToken ct = default)
        => await _dbSet.Where(m => m.StockQuantity <= m.ReorderPoint).ToListAsync(ct);
}

public class BatchRepository : BaseRepository<Batch>, IBatchRepository
{
    public BatchRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<Batch>> GetByWorkOrderIdAsync(Guid workOrderId, CancellationToken ct = default)
        => await _dbSet.Where(b => b.WorkOrderId == workOrderId).ToListAsync(ct);
}

public class DowntimeRepository : BaseRepository<Downtime>, IDowntimeRepository
{
    public DowntimeRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<Downtime>> GetByMachineIdAsync(Guid machineId, CancellationToken ct = default)
        => await _dbSet.Where(d => d.MachineId == machineId).ToListAsync(ct);

    public async Task<IEnumerable<Downtime>> GetActiveDowntimesAsync(CancellationToken ct = default)
        => await _dbSet.Where(d => d.EndedAt == null).ToListAsync(ct);

    public async Task<IEnumerable<Downtime>> GetByDateRangeAsync(
        Guid machineId, DateTime from, DateTime to, CancellationToken ct = default)
        => await _dbSet
            .Where(d => d.MachineId == machineId && d.StartedAt >= from && d.StartedAt <= to)
            .ToListAsync(ct);
}

public class NonConformanceRepository : BaseRepository<NonConformance>, INonConformanceRepository
{
    public NonConformanceRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<NonConformance>> GetByWorkOrderIdAsync(Guid workOrderId, CancellationToken ct = default)
        => await _dbSet.Where(n => n.WorkOrderId == workOrderId).ToListAsync(ct);

    public async Task<IEnumerable<NonConformance>> GetOpenAsync(CancellationToken ct = default)
        => await _dbSet.Where(n => n.Status != NCStatus.Closed).ToListAsync(ct);

    public async Task<string> GenerateNCNumberAsync(CancellationToken ct = default)
    {
        var count = await _dbSet.CountAsync(ct);
        return $"NC-{DateTime.UtcNow:yyyyMM}-{(count + 1):D4}";
    }
}

public class QualityCheckRepository : BaseRepository<QualityCheck>, IQualityCheckRepository
{
    public QualityCheckRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<QualityCheck>> GetByWorkOrderIdAsync(Guid workOrderId, CancellationToken ct = default)
        => await _dbSet.Where(q => q.WorkOrderId == workOrderId).ToListAsync(ct);

    public async Task<IEnumerable<QualityCheck>> GetByOperationIdAsync(Guid operationId, CancellationToken ct = default)
        => await _dbSet.Where(q => q.OperationId == operationId).ToListAsync(ct);
}
