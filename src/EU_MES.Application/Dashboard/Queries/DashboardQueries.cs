using EU_MES.Application.DTOs;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using MediatR;

namespace EU_MES.Application.Dashboard.Queries;

public record GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>;

public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly IWorkOrderRepository _woRepo;
    private readonly IMachineRepository _machineRepo;
    private readonly INonConformanceRepository _ncRepo;
    private readonly IOperationRepository _opRepo;

    public GetDashboardSummaryQueryHandler(
        IWorkOrderRepository woRepo, IMachineRepository machineRepo,
        INonConformanceRepository ncRepo, IOperationRepository opRepo)
    {
        _woRepo = woRepo;
        _machineRepo = machineRepo;
        _ncRepo = ncRepo;
        _opRepo = opRepo;
    }

    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery q, CancellationToken ct)
    {
        var workOrders = (await _woRepo.GetAllAsync(ct)).ToList();
        var machines = (await _machineRepo.GetAllAsync(ct)).ToList();
        var openNCs = (await _ncRepo.GetOpenAsync(ct)).ToList();
        var allOps = (await _opRepo.GetAllAsync(ct)).ToList();

        var today = DateTime.UtcNow.Date;
        int todayProduction = (int)allOps
            .Where(o => o.CompletedAt.HasValue && o.CompletedAt.Value.Date == today)
            .Sum(o => o.ProducedQuantity);

        // Simple OEE estimate: quality rate of completed work orders
        var completed = workOrders.Where(w => w.Status == WorkOrderStatus.Completed).ToList();
        double overallOee = 0;
        if (completed.Any())
        {
            double totalProduced = (double)completed.Sum(w => w.ProducedQuantity);
            double totalScrap = (double)completed.Sum(w => w.ScrapQuantity);
            overallOee = totalProduced > 0 ? (totalProduced - totalScrap) / totalProduced * 0.85 : 0;
        }

        return new DashboardSummaryDto(
            workOrders.Count,
            workOrders.Count(w => w.Status == WorkOrderStatus.InProgress),
            workOrders.Count(w => w.Status == WorkOrderStatus.Completed),
            machines.Count,
            machines.Count(m => m.Status == MachineStatus.Running),
            machines.Count(m => m.Status == MachineStatus.Breakdown),
            openNCs.Count,
            openNCs.Count(n => n.Severity == NCSeverity.Critical),
            Math.Round(overallOee, 4),
            todayProduction);
    }
}
