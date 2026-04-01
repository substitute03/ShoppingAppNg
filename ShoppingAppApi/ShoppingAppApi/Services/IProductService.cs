using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Models;
using ShoppingAppApi.Requests;

namespace ShoppingAppApi.Services;

public interface IProductService
{
    IReadOnlyList<ProductDto> GetProducts();
    ProductDto? GetProductById(Guid id);
    ProductDto CreateProduct(CreateProductRequest request);
    bool DeleteProduct(Guid id);
}
