using EU_MES.Application.DTOs;
using EU_MES.Application.Workers.Commands;
using EU_MES.Application.Workers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EU_MES.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkersController : ControllerBase
{
    private readonly IMediator _mediator;
    public WorkersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkerDto>>> GetAll()
        => Ok(await _mediator.Send(new GetAllWorkersQuery()));

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkerDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetWorkerByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkerDto>> Create([FromBody] CreateWorkerCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorkerDto>> Update(Guid id, [FromBody] UpdateWorkerRequest req)
        => Ok(await _mediator.Send(new UpdateWorkerCommand(id, req.FirstName, req.LastName, req.Department, req.Shift)));
}

public record UpdateWorkerRequest(string FirstName, string LastName, string Department, EU_MES.Domain.Enums.WorkerShift Shift);
