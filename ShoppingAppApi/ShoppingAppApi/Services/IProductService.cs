using ShoppingAppApi.Contracts;
using ShoppingAppApi.Models;

namespace ShoppingAppApi.Services;

public interface IProductService
{
  IReadOnlyList<Product> GetProducts();
  Product? GetProductById(Guid id);
  Product CreateProduct(ProductDto request);
  bool DeleteProduct(Guid id);
}
