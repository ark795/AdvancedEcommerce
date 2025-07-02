using CatalogService.Application.Commands;
using CatalogService.Infrastructure.Data;
using MediatR;

namespace CatalogService.Application.Handlers;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly CatalogDbContext _db;

    public DeleteProductHandler(CatalogDbContext db) => _db = db;

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _db.Products.FindAsync(new object[] { request.Id }, cancellationToken);
        if (product == null) throw new Exception("Product not found");

        _db.Products.Remove(product);
        await _db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
