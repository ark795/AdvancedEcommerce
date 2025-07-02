using CatalogService.Application.Commands;
using CatalogService.Application.DTOs;
using CatalogService.Contracts.Events;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly CatalogDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateProductHandler(CatalogDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync(cancellationToken);

        var @event = new ProductCreatedEvent
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
        };

        await _publishEndpoint.Publish(@event, cancellationToken);

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
