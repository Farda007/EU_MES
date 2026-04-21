using EU_MES.Application.DTOs;
using EU_MES.Application.Materials.Commands;
using EU_MES.Application.Materials.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EU_MES.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController : ControllerBase
{
    private readonly IMediator _mediator;
    public MaterialsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialDto>>> GetAll()
        => Ok(await _mediator.Send(new GetAllMaterialsQuery()));

    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetMaterialByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MaterialDto>> Create([FromBody] CreateMaterialCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id}/stock")]
    public async Task<ActionResult<MaterialDto>> UpdateStock(Guid id, [FromBody] UpdateStockRequest req)
        => Ok(await _mediator.Send(new UpdateStockCommand(id, req.NewQuantity)));
}

public record UpdateStockRequest(decimal NewQuantity);
