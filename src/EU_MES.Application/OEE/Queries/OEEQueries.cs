using EU_MES.Application.DTOs;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.OEE.Queries;

/// <summary>Query to get OEE report for a machine over a date range.</summary>
public record GetOEEReportQuery(Guid MachineId, DateTime From, DateTime To) : IRequest<OEEReportDto>;

public class GetOEEReportQueryHandler : IRequestHandler<GetOEEReportQuery, OEEReportDto>
{
    private readonly IMachineRepository _machineRepo;
    private readonly IDowntimeRepository _downtimeRepo;
    private readonly IOperationRepository _operationRepo;

    public GetOEEReportQueryHandler(
        IMachineRepository machineRepo,
        IDowntimeRepository downtimeRepo,
        IOperationRepository operationRepo)
    {
        _machineRepo = machineRepo;
        _downtimeRepo = downtimeRepo;
        _operationRepo = operationRepo;
    }

    public async Task<OEEReportDto> Handle(GetOEEReportQuery q, CancellationToken ct)
    {
        var machine = await _machineRepo.GetByIdAsync(q.MachineId, ct)
            ?? throw new KeyNotFoundException($"Machine {q.MachineId} not found.");

        var downtimes = await _downtimeRepo.GetByDateRangeAsync(q.MachineId, q.From, q.To, ct);
        var operations = await _operationRepo.GetByMachineIdAsync(q.MachineId, ct);

        // Filter operations to date range
        var rangeOps = operations.Where(o =>
            o.StartedAt.HasValue && o.StartedAt.Value >= q.From && o.StartedAt.Value <= q.To).ToList();

        // OEE Calculations
        double plannedMinutes = (q.To - q.From).TotalMinutes;

        // Availability: planned time minus downtime
        double downtimeMinutes = downtimes
            .Where(d => d.EndedAt.HasValue)
            .Sum(d => d.DurationMinutes ?? 0);

        double availability = plannedMinutes > 0
            ? Math.Max(0, (plannedMinutes - downtimeMinutes) / plannedMinutes)
            : 0;

        // Performance: actual output vs theoretical (use planned duration as theoretical)
        double theoreticalOutput = rangeOps.Sum(o => o.PlannedDurationMinutes > 0 ? 1.0 : 0);
        double actualOutput = rangeOps.Count(o => o.ActualDurationMinutes.HasValue &&
            o.PlannedDurationMinutes > 0 &&
            o.ActualDurationMinutes.Value <= o.PlannedDurationMinutes);

        double performance = theoreticalOutput > 0 ? actualOutput / theoreticalOutput : 1.0;
        performance = Math.Min(1.0, performance);

        // Quality: good output vs total output
        int totalProduced = (int)rangeOps.Sum(o => o.ProducedQuantity);
        int totalScrap = (int)rangeOps.Sum(o => o.ScrapQuantity);
        int totalGood = totalProduced - totalScrap;

        double quality = totalProduced > 0 ? (double)totalGood / totalProduced : 1.0;
        quality = Math.Max(0, quality);

        double oee = availability * performance * quality;

        return new OEEReportDto(
            machine.Id, machine.Code, machine.Name,
            q.From, q.To,
            plannedMinutes, downtimeMinutes,
            Math.Round(availability, 4),
            Math.Round(performance, 4),
            Math.Round(quality, 4),
            Math.Round(oee, 4),
            totalProduced, totalGood, totalScrap);
    }
}
