using CatalogService.Application.Commands;
using CatalogService.Application.DTOs;
using CatalogService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly CatalogDbContext _db;

    public UpdateProductHandler(CatalogDbContext db) => _db = db;

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (product == null) throw new Exception("Product Not Found");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;

        await _db.SaveChangesAsync(cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
        };
    }
}
