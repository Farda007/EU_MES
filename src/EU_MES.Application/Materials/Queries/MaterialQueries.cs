using EU_MES.Application.DTOs;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Materials.Queries;

public record GetAllMaterialsQuery : IRequest<IEnumerable<MaterialDto>>;

public class GetAllMaterialsQueryHandler : IRequestHandler<GetAllMaterialsQuery, IEnumerable<MaterialDto>>
{
    private readonly IMaterialRepository _repo;
    public GetAllMaterialsQueryHandler(IMaterialRepository repo) => _repo = repo;

    public async Task<IEnumerable<MaterialDto>> Handle(GetAllMaterialsQuery q, CancellationToken ct)
    {
        var materials = await _repo.GetAllAsync(ct);
        return materials.Select(m => m.ToDto());
    }
}

public record GetMaterialByIdQuery(Guid Id) : IRequest<MaterialDto?>;

public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, MaterialDto?>
{
    private readonly IMaterialRepository _repo;
    public GetMaterialByIdQueryHandler(IMaterialRepository repo) => _repo = repo;

    public async Task<MaterialDto?> Handle(GetMaterialByIdQuery q, CancellationToken ct)
    {
        var m = await _repo.GetByIdAsync(q.Id, ct);
        return m?.ToDto();
    }
}
