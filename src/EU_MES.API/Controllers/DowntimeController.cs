using EU_MES.Application.DTOs;
using EU_MES.Application.Downtime.Commands;
using EU_MES.Application.Downtime.Queries;
using EU_MES.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EU_MES.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DowntimeController : ControllerBase
{
    private readonly IMediator _mediator;
    public DowntimeController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DowntimeDto>>> GetAll()
        => Ok(await _mediator.Send(new GetAllDowntimesQuery()));

    [HttpGet("by-machine/{machineId}")]
    public async Task<ActionResult<IEnumerable<DowntimeDto>>> GetByMachine(Guid machineId)
        => Ok(await _mediator.Send(new GetDowntimeByMachineQuery(machineId)));

    [HttpGet("report")]
    public async Task<ActionResult<IEnumerable<DowntimeDto>>> GetReport(
        [FromQuery] Guid machineId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        => Ok(await _mediator.Send(new GetDowntimeReportQuery(machineId, from, to)));

    [HttpPost("start")]
    public async Task<ActionResult<DowntimeDto>> Start([FromBody] StartDowntimeRequest req)
    {
        var result = await _mediator.Send(new StartDowntimeCommand(
            req.MachineId, req.Reason, req.Category, req.WorkOrderId, req.ReportedByWorkerId));
        return Ok(result);
    }

    [HttpPost("{id}/end")]
    public async Task<ActionResult<DowntimeDto>> End(Guid id)
        => Ok(await _mediator.Send(new EndDowntimeCommand(id)));
}

public record StartDowntimeRequest(
    Guid MachineId, string Reason, DowntimeCategory Category,
    Guid? WorkOrderId, Guid? ReportedByWorkerId);
