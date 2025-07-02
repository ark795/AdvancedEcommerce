using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.Queries;

public class GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;

