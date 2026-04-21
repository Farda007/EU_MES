using EU_MES.Application.DTOs;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.WorkOrders.Queries;

public record GetWorkOrderByIdQuery(Guid Id) : IRequest<WorkOrderDto?>;

public class GetWorkOrderByIdQueryHandler : IRequestHandler<GetWorkOrderByIdQuery, WorkOrderDto?>
{
    private readonly IWorkOrderRepository _repo;
    public GetWorkOrderByIdQueryHandler(IWorkOrderRepository repo) => _repo = repo;

    public async Task<WorkOrderDto?> Handle(GetWorkOrderByIdQuery q, CancellationToken ct)
    {
        var wo = await _repo.GetByIdAsync(q.Id, ct);
        return wo?.ToDto();
    }
}

public record GetAllWorkOrdersQuery : IRequest<IEnumerable<WorkOrderDto>>;

public class GetAllWorkOrdersQueryHandler : IRequestHandler<GetAllWorkOrdersQuery, IEnumerable<WorkOrderDto>>
{
    private readonly IWorkOrderRepository _repo;
    public GetAllWorkOrdersQueryHandler(IWorkOrderRepository repo) => _repo = repo;

    public async Task<IEnumerable<WorkOrderDto>> Handle(GetAllWorkOrdersQuery q, CancellationToken ct)
    {
        var wos = await _repo.GetAllAsync(ct);
        return wos.Select(w => w.ToDto());
    }
}

public record GetWorkOrdersWithFiltersQuery(
    WorkOrderStatus? Status, WorkOrderPriority? Priority,
    DateTime? From, DateTime? To) : IRequest<IEnumerable<WorkOrderDto>>;

public class GetWorkOrdersWithFiltersQueryHandler
    : IRequestHandler<GetWorkOrdersWithFiltersQuery, IEnumerable<WorkOrderDto>>
{
    private readonly IWorkOrderRepository _repo;
    public GetWorkOrdersWithFiltersQueryHandler(IWorkOrderRepository repo) => _repo = repo;

    public async Task<IEnumerable<WorkOrderDto>> Handle(GetWorkOrdersWithFiltersQuery q, CancellationToken ct)
    {
        var wos = await _repo.GetByFiltersAsync(q.Status, q.Priority, q.From, q.To, ct);
        return wos.Select(w => w.ToDto());
    }
}
