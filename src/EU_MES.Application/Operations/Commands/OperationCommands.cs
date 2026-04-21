using EU_MES.Application.DTOs;
using EU_MES.Domain.Entities;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Operations.Commands;

public record CreateOperationCommand(
    Guid WorkOrderId, int OperationNumber, string Name, string Description,
    Guid MachineId, int PlannedDurationMinutes, Guid? WorkerId) : IRequest<OperationDto>;

public class CreateOperationCommandHandler : IRequestHandler<CreateOperationCommand, OperationDto>
{
    private readonly IOperationRepository _repo;
    public CreateOperationCommandHandler(IOperationRepository repo) => _repo = repo;

    public async Task<OperationDto> Handle(CreateOperationCommand cmd, CancellationToken ct)
    {
        var op = Operation.Create(cmd.WorkOrderId, cmd.OperationNumber, cmd.Name,
            cmd.Description, cmd.MachineId, cmd.PlannedDurationMinutes, cmd.WorkerId);
        await _repo.AddAsync(op, ct);
        await _repo.SaveChangesAsync(ct);
        return op.ToDto();
    }
}

public record StartOperationCommand(Guid Id) : IRequest<OperationDto>;

public class StartOperationCommandHandler : IRequestHandler<StartOperationCommand, OperationDto>
{
    private readonly IOperationRepository _repo;
    public StartOperationCommandHandler(IOperationRepository repo) => _repo = repo;

    public async Task<OperationDto> Handle(StartOperationCommand cmd, CancellationToken ct)
    {
        var op = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Operation {cmd.Id} not found.");
        op.Start();
        await _repo.UpdateAsync(op, ct);
        await _repo.SaveChangesAsync(ct);
        return op.ToDto();
    }
}

public record CompleteOperationCommand(Guid Id, decimal ProducedQuantity, decimal ScrapQuantity) : IRequest<OperationDto>;

public class CompleteOperationCommandHandler : IRequestHandler<CompleteOperationCommand, OperationDto>
{
    private readonly IOperationRepository _repo;
    public CompleteOperationCommandHandler(IOperationRepository repo) => _repo = repo;

    public async Task<OperationDto> Handle(CompleteOperationCommand cmd, CancellationToken ct)
    {
        var op = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Operation {cmd.Id} not found.");
        op.Complete(cmd.ProducedQuantity, cmd.ScrapQuantity);
        await _repo.UpdateAsync(op, ct);
        await _repo.SaveChangesAsync(ct);
        return op.ToDto();
    }
}

public record PauseOperationCommand(Guid Id) : IRequest<OperationDto>;

public class PauseOperationCommandHandler : IRequestHandler<PauseOperationCommand, OperationDto>
{
    private readonly IOperationRepository _repo;
    public PauseOperationCommandHandler(IOperationRepository repo) => _repo = repo;

    public async Task<OperationDto> Handle(PauseOperationCommand cmd, CancellationToken ct)
    {
        var op = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Operation {cmd.Id} not found.");
        op.Pause();
        await _repo.UpdateAsync(op, ct);
        await _repo.SaveChangesAsync(ct);
        return op.ToDto();
    }
}
