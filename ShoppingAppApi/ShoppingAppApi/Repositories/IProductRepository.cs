using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? GetById(int id);
    Product Add(Product product);
    bool Delete(int id);
}
