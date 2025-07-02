using MediatR;

namespace CatalogService.Application.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;

