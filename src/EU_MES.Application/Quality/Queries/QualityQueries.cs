using EU_MES.Application.DTOs;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Quality.Queries;

public record GetNCsByWorkOrderQuery(Guid WorkOrderId) : IRequest<IEnumerable<NonConformanceDto>>;

public class GetNCsByWorkOrderQueryHandler
    : IRequestHandler<GetNCsByWorkOrderQuery, IEnumerable<NonConformanceDto>>
{
    private readonly INonConformanceRepository _repo;
    public GetNCsByWorkOrderQueryHandler(INonConformanceRepository repo) => _repo = repo;

    public async Task<IEnumerable<NonConformanceDto>> Handle(GetNCsByWorkOrderQuery q, CancellationToken ct)
    {
        var ncs = await _repo.GetByWorkOrderIdAsync(q.WorkOrderId, ct);
        return ncs.Select(n => n.ToDto());
    }
}

public record GetOpenNCsQuery : IRequest<IEnumerable<NonConformanceDto>>;

public class GetOpenNCsQueryHandler : IRequestHandler<GetOpenNCsQuery, IEnumerable<NonConformanceDto>>
{
    private readonly INonConformanceRepository _repo;
    public GetOpenNCsQueryHandler(INonConformanceRepository repo) => _repo = repo;

    public async Task<IEnumerable<NonConformanceDto>> Handle(GetOpenNCsQuery q, CancellationToken ct)
    {
        var ncs = await _repo.GetOpenAsync(ct);
        return ncs.Select(n => n.ToDto());
    }
}

public record GetAllNCsQuery : IRequest<IEnumerable<NonConformanceDto>>;

public class GetAllNCsQueryHandler : IRequestHandler<GetAllNCsQuery, IEnumerable<NonConformanceDto>>
{
    private readonly INonConformanceRepository _repo;
    public GetAllNCsQueryHandler(INonConformanceRepository repo) => _repo = repo;

    public async Task<IEnumerable<NonConformanceDto>> Handle(GetAllNCsQuery q, CancellationToken ct)
    {
        var ncs = await _repo.GetAllAsync(ct);
        return ncs.Select(n => n.ToDto());
    }
}

public record GetQualityChecksByWorkOrderQuery(Guid WorkOrderId) : IRequest<IEnumerable<QualityCheckDto>>;

public class GetQualityChecksByWorkOrderQueryHandler
    : IRequestHandler<GetQualityChecksByWorkOrderQuery, IEnumerable<QualityCheckDto>>
{
    private readonly IQualityCheckRepository _repo;
    public GetQualityChecksByWorkOrderQueryHandler(IQualityCheckRepository repo) => _repo = repo;

    public async Task<IEnumerable<QualityCheckDto>> Handle(GetQualityChecksByWorkOrderQuery q, CancellationToken ct)
    {
        var qcs = await _repo.GetByWorkOrderIdAsync(q.WorkOrderId, ct);
        return qcs.Select(q2 => q2.ToDto());
    }
}

public record GetAllQualityChecksQuery : IRequest<IEnumerable<QualityCheckDto>>;

public class GetAllQualityChecksQueryHandler : IRequestHandler<GetAllQualityChecksQuery, IEnumerable<QualityCheckDto>>
{
    private readonly IQualityCheckRepository _repo;
    public GetAllQualityChecksQueryHandler(IQualityCheckRepository repo) => _repo = repo;

    public async Task<IEnumerable<QualityCheckDto>> Handle(GetAllQualityChecksQuery q, CancellationToken ct)
    {
        var qcs = await _repo.GetAllAsync(ct);
        return qcs.Select(q2 => q2.ToDto());
    }
}
