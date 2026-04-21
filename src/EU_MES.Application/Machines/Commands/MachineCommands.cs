using EU_MES.Application.DTOs;
using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Machines.Commands;

public record CreateMachineCommand(string Code, string Name, string Description, string WorkCenter) : IRequest<MachineDto>;

public class CreateMachineCommandHandler : IRequestHandler<CreateMachineCommand, MachineDto>
{
    private readonly IMachineRepository _repo;
    public CreateMachineCommandHandler(IMachineRepository repo) => _repo = repo;

    public async Task<MachineDto> Handle(CreateMachineCommand cmd, CancellationToken ct)
    {
        var m = Machine.Create(cmd.Code, cmd.Name, cmd.Description, cmd.WorkCenter);
        await _repo.AddAsync(m, ct);
        await _repo.SaveChangesAsync(ct);
        return m.ToDto();
    }
}

public record UpdateMachineStatusCommand(Guid Id, MachineStatus Status) : IRequest<MachineDto>;

public class UpdateMachineStatusCommandHandler : IRequestHandler<UpdateMachineStatusCommand, MachineDto>
{
    private readonly IMachineRepository _repo;
    public UpdateMachineStatusCommandHandler(IMachineRepository repo) => _repo = repo;

    public async Task<MachineDto> Handle(UpdateMachineStatusCommand cmd, CancellationToken ct)
    {
        var m = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Machine {cmd.Id} not found.");
        m.UpdateStatus(cmd.Status);
        await _repo.UpdateAsync(m, ct);
        await _repo.SaveChangesAsync(ct);
        return m.ToDto();
    }
}

public record UpdateMachineCommand(Guid Id, string Name, string Description, string WorkCenter) : IRequest<MachineDto>;

public class UpdateMachineCommandHandler : IRequestHandler<UpdateMachineCommand, MachineDto>
{
    private readonly IMachineRepository _repo;
    public UpdateMachineCommandHandler(IMachineRepository repo) => _repo = repo;

    public async Task<MachineDto> Handle(UpdateMachineCommand cmd, CancellationToken ct)
    {
        var m = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Machine {cmd.Id} not found.");
        m.Update(cmd.Name, cmd.Description, cmd.WorkCenter);
        await _repo.UpdateAsync(m, ct);
        await _repo.SaveChangesAsync(ct);
        return m.ToDto();
    }
}
