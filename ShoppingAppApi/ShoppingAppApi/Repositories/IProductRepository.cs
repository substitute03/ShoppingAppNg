using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? GetById(Guid id);
    Product? GetByIdempotencyToken(Guid idempotencyToken);
    Product Add(Product product);
    bool Delete(Guid id);
}
