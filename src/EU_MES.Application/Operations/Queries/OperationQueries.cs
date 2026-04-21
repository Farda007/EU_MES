using EU_MES.Application.DTOs;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Operations.Queries;

public record GetOperationsByWorkOrderQuery(Guid WorkOrderId) : IRequest<IEnumerable<OperationDto>>;

public class GetOperationsByWorkOrderQueryHandler
    : IRequestHandler<GetOperationsByWorkOrderQuery, IEnumerable<OperationDto>>
{
    private readonly IOperationRepository _repo;
    public GetOperationsByWorkOrderQueryHandler(IOperationRepository repo) => _repo = repo;

    public async Task<IEnumerable<OperationDto>> Handle(GetOperationsByWorkOrderQuery q, CancellationToken ct)
    {
        var ops = await _repo.GetByWorkOrderIdAsync(q.WorkOrderId, ct);
        return ops.Select(o => o.ToDto()).OrderBy(o => o.OperationNumber);
    }
}

public record GetOperationByIdQuery(Guid Id) : IRequest<OperationDto?>;

public class GetOperationByIdQueryHandler : IRequestHandler<GetOperationByIdQuery, OperationDto?>
{
    private readonly IOperationRepository _repo;
    public GetOperationByIdQueryHandler(IOperationRepository repo) => _repo = repo;

    public async Task<OperationDto?> Handle(GetOperationByIdQuery q, CancellationToken ct)
    {
        var op = await _repo.GetByIdAsync(q.Id, ct);
        return op?.ToDto();
    }
}
