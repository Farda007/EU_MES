using EU_MES.Application.DTOs;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Workers.Queries;

public record GetAllWorkersQuery : IRequest<IEnumerable<WorkerDto>>;

public class GetAllWorkersQueryHandler : IRequestHandler<GetAllWorkersQuery, IEnumerable<WorkerDto>>
{
    private readonly IWorkerRepository _repo;
    public GetAllWorkersQueryHandler(IWorkerRepository repo) => _repo = repo;

    public async Task<IEnumerable<WorkerDto>> Handle(GetAllWorkersQuery q, CancellationToken ct)
    {
        var workers = await _repo.GetAllAsync(ct);
        return workers.Select(w => w.ToDto());
    }
}

public record GetWorkerByIdQuery(Guid Id) : IRequest<WorkerDto?>;

public class GetWorkerByIdQueryHandler : IRequestHandler<GetWorkerByIdQuery, WorkerDto?>
{
    private readonly IWorkerRepository _repo;
    public GetWorkerByIdQueryHandler(IWorkerRepository repo) => _repo = repo;

    public async Task<WorkerDto?> Handle(GetWorkerByIdQuery q, CancellationToken ct)
    {
        var w = await _repo.GetByIdAsync(q.Id, ct);
        return w?.ToDto();
    }
}
