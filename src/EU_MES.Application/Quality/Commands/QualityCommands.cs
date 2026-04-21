using EU_MES.Application.DTOs;
using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.Quality.Commands;

public record CreateNonConformanceCommand(
    NCType Type, NCSeverity Severity, string Description,
    Guid? WorkOrderId, Guid? OperationId, Guid? MachineId, Guid? WorkerId) : IRequest<NonConformanceDto>;

public class CreateNonConformanceCommandHandler : IRequestHandler<CreateNonConformanceCommand, NonConformanceDto>
{
    private readonly INonConformanceRepository _repo;
    public CreateNonConformanceCommandHandler(INonConformanceRepository repo) => _repo = repo;

    public async Task<NonConformanceDto> Handle(CreateNonConformanceCommand cmd, CancellationToken ct)
    {
        var ncNumber = await _repo.GenerateNCNumberAsync(ct);
        var nc = NonConformance.Create(ncNumber, cmd.Type, cmd.Severity, cmd.Description,
            cmd.WorkOrderId, cmd.OperationId, cmd.MachineId, cmd.WorkerId);
        await _repo.AddAsync(nc, ct);
        await _repo.SaveChangesAsync(ct);
        return nc.ToDto();
    }
}

public record CloseNonConformanceCommand(Guid Id, string RootCause, string CorrectiveAction) : IRequest<NonConformanceDto>;

public class CloseNonConformanceCommandHandler : IRequestHandler<CloseNonConformanceCommand, NonConformanceDto>
{
    private readonly INonConformanceRepository _repo;
    public CloseNonConformanceCommandHandler(INonConformanceRepository repo) => _repo = repo;

    public async Task<NonConformanceDto> Handle(CloseNonConformanceCommand cmd, CancellationToken ct)
    {
        var nc = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"NC {cmd.Id} not found.");
        nc.Close(cmd.RootCause, cmd.CorrectiveAction);
        await _repo.UpdateAsync(nc, ct);
        await _repo.SaveChangesAsync(ct);
        return nc.ToDto();
    }
}

public record CreateQualityCheckCommand(
    Guid WorkOrderId, string CheckName, QualityCheckType CheckType,
    Guid? OperationId, decimal? MinValue, decimal? MaxValue,
    Guid? InspectorWorkerId) : IRequest<QualityCheckDto>;

public class CreateQualityCheckCommandHandler : IRequestHandler<CreateQualityCheckCommand, QualityCheckDto>
{
    private readonly IQualityCheckRepository _repo;
    public CreateQualityCheckCommandHandler(IQualityCheckRepository repo) => _repo = repo;

    public async Task<QualityCheckDto> Handle(CreateQualityCheckCommand cmd, CancellationToken ct)
    {
        var qc = QualityCheck.Create(cmd.WorkOrderId, cmd.CheckName, cmd.CheckType,
            cmd.OperationId, cmd.MinValue, cmd.MaxValue, cmd.InspectorWorkerId);
        await _repo.AddAsync(qc, ct);
        await _repo.SaveChangesAsync(ct);
        return qc.ToDto();
    }
}

public record UpdateQualityCheckResultCommand(
    Guid Id, QualityCheckResult Result, decimal? MeasuredValue, string? Notes) : IRequest<QualityCheckDto>;

public class UpdateQualityCheckResultCommandHandler : IRequestHandler<UpdateQualityCheckResultCommand, QualityCheckDto>
{
    private readonly IQualityCheckRepository _repo;
    public UpdateQualityCheckResultCommandHandler(IQualityCheckRepository repo) => _repo = repo;

    public async Task<QualityCheckDto> Handle(UpdateQualityCheckResultCommand cmd, CancellationToken ct)
    {
        var qc = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"QualityCheck {cmd.Id} not found.");
        qc.SetResult(cmd.Result, cmd.MeasuredValue, cmd.Notes);
        await _repo.UpdateAsync(qc, ct);
        await _repo.SaveChangesAsync(ct);
        return qc.ToDto();
    }
}
