using EU_MES.Application.DTOs;
using EU_MES.Application.WorkOrders.Commands;
using EU_MES.Application.WorkOrders.Queries;
using EU_MES.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EU_MES.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    public WorkOrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetAll(
        [FromQuery] WorkOrderStatus? status, [FromQuery] WorkOrderPriority? priority,
        [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        if (status.HasValue || priority.HasValue || from.HasValue || to.HasValue)
            return Ok(await _mediator.Send(new GetWorkOrdersWithFiltersQuery(status, priority, from, to)));
        return Ok(await _mediator.Send(new GetAllWorkOrdersQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkOrderDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetWorkOrderByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkOrderDto>> Create([FromBody] CreateWorkOrderCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorkOrderDto>> Update(Guid id, [FromBody] UpdateWorkOrderRequest req)
    {
        var cmd = new UpdateWorkOrderCommand(id, req.ProductCode, req.ProductName, req.PlannedQuantity,
            req.Priority, req.PlannedStartDate, req.PlannedEndDate, req.MachineId);
        return Ok(await _mediator.Send(cmd));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteWorkOrderCommand(id));
        return NoContent();
    }

    [HttpPost("{id}/start")]
    public async Task<ActionResult<WorkOrderDto>> Start(Guid id)
        => Ok(await _mediator.Send(new StartWorkOrderCommand(id)));

    [HttpPost("{id}/complete")]
    public async Task<ActionResult<WorkOrderDto>> Complete(Guid id)
        => Ok(await _mediator.Send(new CompleteWorkOrderCommand(id)));
}

public record UpdateWorkOrderRequest(
    string ProductCode, string ProductName, decimal PlannedQuantity,
    WorkOrderPriority Priority, DateTime PlannedStartDate, DateTime PlannedEndDate, Guid? MachineId);
