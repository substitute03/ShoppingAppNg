using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public interface IProductRepository
{
    IReadOnlyList<ProductDto> GetAll();
    ProductDto? GetById(Guid id);
    ProductDto? GetByIdempotencyToken(Guid idempotencyToken);
    ProductDto Add(Product product);
    bool Delete(Guid id);
}
