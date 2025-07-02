using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.Commands;

public record CreateProductCommand(string Name, string Description, decimal Price, int Stock) 
    : IRequest<ProductDto>;

