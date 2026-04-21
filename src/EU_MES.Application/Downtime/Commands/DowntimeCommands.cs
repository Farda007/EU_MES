using EU_MES.Application.DTOs;
using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Downtime.Commands;

public record StartDowntimeCommand(
    Guid MachineId, string Reason, DowntimeCategory Category,
    Guid? WorkOrderId = null, Guid? ReportedByWorkerId = null) : IRequest<DowntimeDto>;

public class StartDowntimeCommandHandler : IRequestHandler<StartDowntimeCommand, DowntimeDto>
{
    private readonly IDowntimeRepository _repo;
    public StartDowntimeCommandHandler(IDowntimeRepository repo) => _repo = repo;

    public async Task<DowntimeDto> Handle(StartDowntimeCommand cmd, CancellationToken ct)
    {
        var dt = Domain.Entities.Downtime.Create(cmd.MachineId, cmd.Reason, cmd.Category,
            cmd.WorkOrderId, cmd.ReportedByWorkerId);
        await _repo.AddAsync(dt, ct);
        await _repo.SaveChangesAsync(ct);
        return dt.ToDto();
    }
}

public record EndDowntimeCommand(Guid Id) : IRequest<DowntimeDto>;

public class EndDowntimeCommandHandler : IRequestHandler<EndDowntimeCommand, DowntimeDto>
{
    private readonly IDowntimeRepository _repo;
    public EndDowntimeCommandHandler(IDowntimeRepository repo) => _repo = repo;

    public async Task<DowntimeDto> Handle(EndDowntimeCommand cmd, CancellationToken ct)
    {
        var dt = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Downtime {cmd.Id} not found.");
        dt.End();
        await _repo.UpdateAsync(dt, ct);
        await _repo.SaveChangesAsync(ct);
        return dt.ToDto();
    }
}
