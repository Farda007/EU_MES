using EU_MES.Application.DTOs;
using EU_MES.Application.OEE.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EU_MES.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OEEController : ControllerBase
{
    private readonly IMediator _mediator;
    public OEEController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get OEE report for a machine over a date range.</summary>
    [HttpGet]
    public async Task<ActionResult<OEEReportDto>> GetOEE(
        [FromQuery] Guid machineId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        => Ok(await _mediator.Send(new GetOEEReportQuery(machineId, from, to)));
}
