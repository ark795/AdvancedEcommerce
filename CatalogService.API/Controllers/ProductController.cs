using CatalogService.Application.Commands;
using CatalogService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetAllProductsQuery()));

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command) => Ok(await _mediator.Send(command));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid Id, UpdateProductCommand command)
    {
        if (Id != command.Id) return BadRequest("ID Missmatch");
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}
