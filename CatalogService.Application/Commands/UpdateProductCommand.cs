using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.Commands;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, int Stock)
    : IRequest<ProductDto>;

