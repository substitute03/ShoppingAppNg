using ShoppingAppApi.Models;
using ShoppingAppApi.Requests;

namespace ShoppingAppApi.Services;

public interface IProductService
{
    IReadOnlyList<Product> GetProducts();
    Product? GetProductById(Guid id);
    Product CreateProduct(CreateProductRequest request);
    bool DeleteProduct(Guid id);
}
