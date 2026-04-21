using EU_MES.Application.DTOs;
using EU_MES.Application.Quality.Commands;
using EU_MES.Application.Quality.Queries;
using EU_MES.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EU_MES.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QualityController : ControllerBase
{
    private readonly IMediator _mediator;
    public QualityController(IMediator mediator) => _mediator = mediator;

    // Non-Conformances
    [HttpGet("nc")]
    public async Task<ActionResult<IEnumerable<NonConformanceDto>>> GetAllNCs()
        => Ok(await _mediator.Send(new GetAllNCsQuery()));

    [HttpGet("nc/open")]
    public async Task<ActionResult<IEnumerable<NonConformanceDto>>> GetOpenNCs()
        => Ok(await _mediator.Send(new GetOpenNCsQuery()));

    [HttpGet("nc/by-workorder/{workOrderId}")]
    public async Task<ActionResult<IEnumerable<NonConformanceDto>>> GetNCsByWorkOrder(Guid workOrderId)
        => Ok(await _mediator.Send(new GetNCsByWorkOrderQuery(workOrderId)));

    [HttpPost("nc")]
    public async Task<ActionResult<NonConformanceDto>> CreateNC([FromBody] CreateNonConformanceCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPost("nc/{id}/close")]
    public async Task<ActionResult<NonConformanceDto>> CloseNC(Guid id, [FromBody] CloseNCRequest req)
        => Ok(await _mediator.Send(new CloseNonConformanceCommand(id, req.RootCause, req.CorrectiveAction)));

    // Quality Checks
    [HttpGet("checks")]
    public async Task<ActionResult<IEnumerable<QualityCheckDto>>> GetAllChecks()
        => Ok(await _mediator.Send(new GetAllQualityChecksQuery()));

    [HttpGet("checks/by-workorder/{workOrderId}")]
    public async Task<ActionResult<IEnumerable<QualityCheckDto>>> GetChecksByWorkOrder(Guid workOrderId)
        => Ok(await _mediator.Send(new GetQualityChecksByWorkOrderQuery(workOrderId)));

    [HttpPost("checks")]
    public async Task<ActionResult<QualityCheckDto>> CreateCheck([FromBody] CreateQualityCheckCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPatch("checks/{id}/result")]
    public async Task<ActionResult<QualityCheckDto>> UpdateResult(Guid id, [FromBody] UpdateResultRequest req)
        => Ok(await _mediator.Send(new UpdateQualityCheckResultCommand(id, req.Result, req.MeasuredValue, req.Notes)));
}

public record CloseNCRequest(string RootCause, string CorrectiveAction);
public record UpdateResultRequest(QualityCheckResult Result, decimal? MeasuredValue, string? Notes);
