using EU_MES.Application.DTOs;
using EU_MES.Application.Machines.Commands;
using EU_MES.Application.Machines.Queries;
using EU_MES.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EU_MES.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachinesController : ControllerBase
{
    private readonly IMediator _mediator;
    public MachinesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MachineDto>>> GetAll()
        => Ok(await _mediator.Send(new GetAllMachinesQuery()));

    [HttpGet("{id}")]
    public async Task<ActionResult<MachineDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetMachineByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MachineDto>> Create([FromBody] CreateMachineCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MachineDto>> Update(Guid id, [FromBody] UpdateMachineRequest req)
        => Ok(await _mediator.Send(new UpdateMachineCommand(id, req.Name, req.Description, req.WorkCenter)));

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<MachineDto>> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest req)
        => Ok(await _mediator.Send(new UpdateMachineStatusCommand(id, req.Status)));
}

public record UpdateMachineRequest(string Name, string Description, string WorkCenter);
public record UpdateStatusRequest(MachineStatus Status);
