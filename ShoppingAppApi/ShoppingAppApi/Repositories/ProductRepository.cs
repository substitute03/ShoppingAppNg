using System.Collections.Concurrent;
using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public class ProductRepository : IProductRepository
{
    private static readonly Guid LaptopId = Guid.Parse("a0000000-0000-4000-8000-000000000001");
    private static readonly Guid HeadphonesId = Guid.Parse("a0000000-0000-4000-8000-000000000002");
    private static readonly Guid KeyboardId = Guid.Parse("a0000000-0000-4000-8000-000000000003");

    private static readonly ConcurrentDictionary<Guid, Product> Products = new();
    private static readonly ConcurrentDictionary<Guid, Guid> ProductIdByIdempotencyToken = new();

    static ProductRepository()
    {
        Products[LaptopId] = new Product { Id = LaptopId, Name = "Laptop", Price = 1299.99m };
        Products[HeadphonesId] = new Product { Id = HeadphonesId, Name = "Headphones", Price = 149.99m };
        Products[KeyboardId] = new Product { Id = KeyboardId, Name = "Keyboard", Price = 49.99m };
    }

    public IReadOnlyList<ProductDto> GetAll()
    {
        return Products.Values
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();
    }

    public ProductDto? GetById(Guid id)
    {
        return Products.Values
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).SingleOrDefault() ?? null;
    }

    public ProductDto? GetByIdempotencyToken(Guid idempotencyToken)
    {
        if (idempotencyToken == Guid.Empty)
        {
            return null;
        }

        if (!ProductIdByIdempotencyToken.TryGetValue(idempotencyToken, out var productId))
        {
            return null;
        }

        return GetById(productId);
    }

    public ProductDto Add(Product product)
    {
        bool isDuplicateProduct =
            (product.Id != Guid.Empty && Products.ContainsKey(product.Id)) ||
            (product.IdempotencyToken != Guid.Empty &&
             ProductIdByIdempotencyToken.ContainsKey(product.IdempotencyToken));

        if (isDuplicateProduct)
        {
            throw new InvalidOperationException("Product already exists");
        }

        var newId = Guid.NewGuid();
        var productToSave = new Product
        {
            Id = newId,
            Name = product.Name,
            Price = product.Price,
            IdempotencyToken = product.IdempotencyToken
        };

        Products[newId] = productToSave;

        if (product.IdempotencyToken != Guid.Empty)
        {
            ProductIdByIdempotencyToken[product.IdempotencyToken] = newId;
        }

        return new ProductDto
        {
            Id = newId,
            Name = productToSave.Name,
            Price = productToSave.Price 
        };
    }

    public bool Delete(Guid id)
    {
        if (!Products.TryRemove(id, out var product))
        {
            return false;
        }

        if (product.IdempotencyToken != Guid.Empty)
        {
            ProductIdByIdempotencyToken.TryRemove(product.IdempotencyToken, out _);
        }

        return true;
    }
}
