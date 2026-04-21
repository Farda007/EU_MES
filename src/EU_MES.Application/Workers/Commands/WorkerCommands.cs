using EU_MES.Application.DTOs;
using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Workers.Commands;

public record CreateWorkerCommand(
    string EmployeeNumber, string FirstName, string LastName,
    string Department, WorkerShift Shift) : IRequest<WorkerDto>;

public class CreateWorkerCommandHandler : IRequestHandler<CreateWorkerCommand, WorkerDto>
{
    private readonly IWorkerRepository _repo;
    public CreateWorkerCommandHandler(IWorkerRepository repo) => _repo = repo;

    public async Task<WorkerDto> Handle(CreateWorkerCommand cmd, CancellationToken ct)
    {
        var w = Worker.Create(cmd.EmployeeNumber, cmd.FirstName, cmd.LastName, cmd.Department, cmd.Shift);
        await _repo.AddAsync(w, ct);
        await _repo.SaveChangesAsync(ct);
        return w.ToDto();
    }
}

public record UpdateWorkerCommand(
    Guid Id, string FirstName, string LastName,
    string Department, WorkerShift Shift) : IRequest<WorkerDto>;

public class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, WorkerDto>
{
    private readonly IWorkerRepository _repo;
    public UpdateWorkerCommandHandler(IWorkerRepository repo) => _repo = repo;

    public async Task<WorkerDto> Handle(UpdateWorkerCommand cmd, CancellationToken ct)
    {
        var w = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Worker {cmd.Id} not found.");
        w.Update(cmd.FirstName, cmd.LastName, cmd.Department, cmd.Shift);
        await _repo.UpdateAsync(w, ct);
        await _repo.SaveChangesAsync(ct);
        return w.ToDto();
    }
}
