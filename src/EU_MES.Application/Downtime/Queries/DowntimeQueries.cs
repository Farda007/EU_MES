using EU_MES.Application.DTOs;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Downtime.Queries;

public record GetDowntimeByMachineQuery(Guid MachineId) : IRequest<IEnumerable<DowntimeDto>>;

public class GetDowntimeByMachineQueryHandler
    : IRequestHandler<GetDowntimeByMachineQuery, IEnumerable<DowntimeDto>>
{
    private readonly IDowntimeRepository _repo;
    public GetDowntimeByMachineQueryHandler(IDowntimeRepository repo) => _repo = repo;

    public async Task<IEnumerable<DowntimeDto>> Handle(GetDowntimeByMachineQuery q, CancellationToken ct)
    {
        var dts = await _repo.GetByMachineIdAsync(q.MachineId, ct);
        return dts.Select(d => d.ToDto()).OrderByDescending(d => d.StartedAt);
    }
}

public record GetDowntimeReportQuery(Guid MachineId, DateTime From, DateTime To)
    : IRequest<IEnumerable<DowntimeDto>>;

public class GetDowntimeReportQueryHandler
    : IRequestHandler<GetDowntimeReportQuery, IEnumerable<DowntimeDto>>
{
    private readonly IDowntimeRepository _repo;
    public GetDowntimeReportQueryHandler(IDowntimeRepository repo) => _repo = repo;

    public async Task<IEnumerable<DowntimeDto>> Handle(GetDowntimeReportQuery q, CancellationToken ct)
    {
        var dts = await _repo.GetByDateRangeAsync(q.MachineId, q.From, q.To, ct);
        return dts.Select(d => d.ToDto()).OrderByDescending(d => d.StartedAt);
    }
}

public record GetAllDowntimesQuery : IRequest<IEnumerable<DowntimeDto>>;

public class GetAllDowntimesQueryHandler : IRequestHandler<GetAllDowntimesQuery, IEnumerable<DowntimeDto>>
{
    private readonly IDowntimeRepository _repo;
    public GetAllDowntimesQueryHandler(IDowntimeRepository repo) => _repo = repo;

    public async Task<IEnumerable<DowntimeDto>> Handle(GetAllDowntimesQuery q, CancellationToken ct)
    {
        var dts = await _repo.GetAllAsync(ct);
        return dts.Select(d => d.ToDto());
    }
}
