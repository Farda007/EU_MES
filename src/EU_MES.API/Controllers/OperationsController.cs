using EU_MES.Application.DTOs;
using EU_MES.Application.Operations.Commands;
using EU_MES.Application.Operations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EU_MES.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperationsController : ControllerBase
{
    private readonly IMediator _mediator;
    public OperationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<ActionResult<OperationDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetOperationByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-workorder/{workOrderId}")]
    public async Task<ActionResult<IEnumerable<OperationDto>>> GetByWorkOrder(Guid workOrderId)
        => Ok(await _mediator.Send(new GetOperationsByWorkOrderQuery(workOrderId)));

    [HttpPost]
    public async Task<ActionResult<OperationDto>> Create([FromBody] CreateOperationCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id}/start")]
    public async Task<ActionResult<OperationDto>> Start(Guid id)
        => Ok(await _mediator.Send(new StartOperationCommand(id)));

    [HttpPost("{id}/pause")]
    public async Task<ActionResult<OperationDto>> Pause(Guid id)
        => Ok(await _mediator.Send(new PauseOperationCommand(id)));

    [HttpPost("{id}/complete")]
    public async Task<ActionResult<OperationDto>> Complete(Guid id, [FromBody] CompleteOpRequest req)
        => Ok(await _mediator.Send(new CompleteOperationCommand(id, req.ProducedQuantity, req.ScrapQuantity)));
}

public record CompleteOpRequest(decimal ProducedQuantity, decimal ScrapQuantity);
