using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Models;

namespace ShoppingAppApi.Services;

public interface IProductService
{
    IReadOnlyList<Product> GetProducts();
    Product? GetProductById(int id);
    Product CreateProduct(ProductDto request);
    bool DeleteProduct(int id);
}
