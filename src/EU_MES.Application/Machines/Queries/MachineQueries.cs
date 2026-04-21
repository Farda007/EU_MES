using EU_MES.Application.DTOs;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Machines.Queries;

public record GetAllMachinesQuery : IRequest<IEnumerable<MachineDto>>;

public class GetAllMachinesQueryHandler : IRequestHandler<GetAllMachinesQuery, IEnumerable<MachineDto>>
{
    private readonly IMachineRepository _repo;
    public GetAllMachinesQueryHandler(IMachineRepository repo) => _repo = repo;

    public async Task<IEnumerable<MachineDto>> Handle(GetAllMachinesQuery q, CancellationToken ct)
    {
        var machines = await _repo.GetAllAsync(ct);
        return machines.Select(m => m.ToDto());
    }
}

public record GetMachineByIdQuery(Guid Id) : IRequest<MachineDto?>;

public class GetMachineByIdQueryHandler : IRequestHandler<GetMachineByIdQuery, MachineDto?>
{
    private readonly IMachineRepository _repo;
    public GetMachineByIdQueryHandler(IMachineRepository repo) => _repo = repo;

    public async Task<MachineDto?> Handle(GetMachineByIdQuery q, CancellationToken ct)
    {
        var m = await _repo.GetByIdAsync(q.Id, ct);
        return m?.ToDto();
    }
}
