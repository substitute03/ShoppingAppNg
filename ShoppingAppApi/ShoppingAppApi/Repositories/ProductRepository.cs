using System.Collections.Concurrent;
using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public class ProductRepository : IProductRepository
{
    private static readonly ConcurrentDictionary<int, Product> Products = new();
    private static readonly ConcurrentDictionary<Guid, int> ProductIdByIdempotencyToken = new();
    private int _nextProductId = Products.Keys.Max() + 1;

    static ProductRepository()
    {
        Products[1] = new Product { Id = 1, Name = "Laptop", Price = 1299.99m };
        Products[2] = new Product { Id = 2, Name = "Headphones", Price = 149.99m };
        Products[3] = new Product { Id = 3, Name = "Keyboard", Price = 49.99m };
    }

    public IReadOnlyList<Product> GetAll()
    {
        return Products.Values.Select(Clone).ToList();
    }

    public Product? GetById(int id)
    {
        return Products.TryGetValue(id, out var product) ? Clone(product) : null;
    }

    public Product? GetByIdempotencyToken(Guid idempotencyToken)
    {
        if (idempotencyToken == Guid.Empty)
        {
            return null;
        }

        int productId = ProductIdByIdempotencyToken
            .GetValueOrDefault(idempotencyToken, -1);

        if (productId == -1)
        {
            return null;
        }

        if (!Products.TryGetValue(productId, out var product))
        {
            return null;
        }

        return Clone(product);
    }

    public Product Add(Product product)
    {
        bool isDuplicateProduct = 
            Products.ContainsKey(product.Id) ||
            (product.IdempotencyToken != Guid.Empty &&
             ProductIdByIdempotencyToken.ContainsKey(product.IdempotencyToken));

        if (isDuplicateProduct)
        {
            throw new InvalidOperationException("Product already exists");
        }

        var productToSave = new Product
        {
            Id = _nextProductId,
            Name = product.Name,
            Price = product.Price,
            IdempotencyToken = product.IdempotencyToken
        };

        // "Save" the product
        Products[_nextProductId] = productToSave;
        ProductIdByIdempotencyToken[product.IdempotencyToken] = _nextProductId;

        return Clone(productToSave);
    }

    public bool Delete(int id)
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

    // Clone the product to avoid returning the same reference as we are using in-memory collections
    private static Product Clone(Product product) =>
        new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            IdempotencyToken = product.IdempotencyToken
        };
}
