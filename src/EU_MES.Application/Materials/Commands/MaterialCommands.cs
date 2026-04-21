using EU_MES.Application.DTOs;
using EU_MES.Domain.Entities;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Materials.Commands;

public record CreateMaterialCommand(
    string Code, string Name, string Unit,
    decimal StockQuantity, decimal ReorderPoint, string Category) : IRequest<MaterialDto>;

public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, MaterialDto>
{
    private readonly IMaterialRepository _repo;
    public CreateMaterialCommandHandler(IMaterialRepository repo) => _repo = repo;

    public async Task<MaterialDto> Handle(CreateMaterialCommand cmd, CancellationToken ct)
    {
        var m = Material.Create(cmd.Code, cmd.Name, cmd.Unit, cmd.StockQuantity, cmd.ReorderPoint, cmd.Category);
        await _repo.AddAsync(m, ct);
        await _repo.SaveChangesAsync(ct);
        return m.ToDto();
    }
}

public record UpdateStockCommand(Guid Id, decimal NewQuantity) : IRequest<MaterialDto>;

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, MaterialDto>
{
    private readonly IMaterialRepository _repo;
    public UpdateStockCommandHandler(IMaterialRepository repo) => _repo = repo;

    public async Task<MaterialDto> Handle(UpdateStockCommand cmd, CancellationToken ct)
    {
        var m = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Material {cmd.Id} not found.");
        m.UpdateStock(cmd.NewQuantity);
        await _repo.UpdateAsync(m, ct);
        await _repo.SaveChangesAsync(ct);
        return m.ToDto();
    }
}
