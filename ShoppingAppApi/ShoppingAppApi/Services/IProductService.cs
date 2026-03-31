using ShoppingAppApi.Models;
using ShoppingAppApi.Requests;

namespace ShoppingAppApi.Services;

public interface IProductService
{
    IReadOnlyList<Product> GetProducts();
    Product? GetProductById(int id);
    Product CreateProduct(CreateProductRequest request);
    bool DeleteProduct(int id);
}
