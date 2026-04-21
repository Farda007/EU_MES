using EU_MES.Application.DTOs;
using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using EU_MES.Application.Common;
using MediatR;

namespace EU_MES.Application.WorkOrders.Commands;

public record CreateWorkOrderCommand(
    string OrderNumber, string ProductCode, string ProductName,
    decimal PlannedQuantity, WorkOrderPriority Priority,
    DateTime PlannedStartDate, DateTime PlannedEndDate, Guid? MachineId) : IRequest<WorkOrderDto>;

public class CreateWorkOrderCommandHandler : IRequestHandler<CreateWorkOrderCommand, WorkOrderDto>
{
    private readonly IWorkOrderRepository _repo;
    public CreateWorkOrderCommandHandler(IWorkOrderRepository repo) => _repo = repo;

    public async Task<WorkOrderDto> Handle(CreateWorkOrderCommand cmd, CancellationToken ct)
    {
        var wo = WorkOrder.Create(cmd.OrderNumber, cmd.ProductCode, cmd.ProductName,
            cmd.PlannedQuantity, cmd.Priority, cmd.PlannedStartDate, cmd.PlannedEndDate, cmd.MachineId);
        await _repo.AddAsync(wo, ct);
        await _repo.SaveChangesAsync(ct);
        return wo.ToDto();
    }
}

public record UpdateWorkOrderCommand(
    Guid Id, string ProductCode, string ProductName, decimal PlannedQuantity,
    WorkOrderPriority Priority, DateTime PlannedStartDate, DateTime PlannedEndDate,
    Guid? MachineId) : IRequest<WorkOrderDto>;

public class UpdateWorkOrderCommandHandler : IRequestHandler<UpdateWorkOrderCommand, WorkOrderDto>
{
    private readonly IWorkOrderRepository _repo;
    public UpdateWorkOrderCommandHandler(IWorkOrderRepository repo) => _repo = repo;

    public async Task<WorkOrderDto> Handle(UpdateWorkOrderCommand cmd, CancellationToken ct)
    {
        var wo = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"WorkOrder {cmd.Id} not found.");
        wo.Update(cmd.ProductCode, cmd.ProductName, cmd.PlannedQuantity,
            cmd.Priority, cmd.PlannedStartDate, cmd.PlannedEndDate, cmd.MachineId);
        await _repo.UpdateAsync(wo, ct);
        await _repo.SaveChangesAsync(ct);
        return wo.ToDto();
    }
}

public record DeleteWorkOrderCommand(Guid Id) : IRequest<bool>;

public class DeleteWorkOrderCommandHandler : IRequestHandler<DeleteWorkOrderCommand, bool>
{
    private readonly IWorkOrderRepository _repo;
    public DeleteWorkOrderCommandHandler(IWorkOrderRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteWorkOrderCommand cmd, CancellationToken ct)
    {
        var wo = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"WorkOrder {cmd.Id} not found.");
        await _repo.DeleteAsync(wo, ct);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}

public record StartWorkOrderCommand(Guid Id) : IRequest<WorkOrderDto>;

public class StartWorkOrderCommandHandler : IRequestHandler<StartWorkOrderCommand, WorkOrderDto>
{
    private readonly IWorkOrderRepository _repo;
    public StartWorkOrderCommandHandler(IWorkOrderRepository repo) => _repo = repo;

    public async Task<WorkOrderDto> Handle(StartWorkOrderCommand cmd, CancellationToken ct)
    {
        var wo = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"WorkOrder {cmd.Id} not found.");
        wo.Start();
        await _repo.UpdateAsync(wo, ct);
        await _repo.SaveChangesAsync(ct);
        return wo.ToDto();
    }
}

public record CompleteWorkOrderCommand(Guid Id) : IRequest<WorkOrderDto>;

public class CompleteWorkOrderCommandHandler : IRequestHandler<CompleteWorkOrderCommand, WorkOrderDto>
{
    private readonly IWorkOrderRepository _repo;
    public CompleteWorkOrderCommandHandler(IWorkOrderRepository repo) => _repo = repo;

    public async Task<WorkOrderDto> Handle(CompleteWorkOrderCommand cmd, CancellationToken ct)
    {
        var wo = await _repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"WorkOrder {cmd.Id} not found.");
        wo.Complete();
        await _repo.UpdateAsync(wo, ct);
        await _repo.SaveChangesAsync(ct);
        return wo.ToDto();
    }
}
