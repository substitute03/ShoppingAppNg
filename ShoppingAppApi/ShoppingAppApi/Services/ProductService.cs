using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Models;
using ShoppingAppApi.Repositories;
using ShoppingAppApi.Requests;
using ShoppingAppApi.Exceptions;

namespace ShoppingAppApi.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public IReadOnlyList<ProductDto> GetProducts()
    {
        return productRepository.GetAll();
    }

    public ProductDto? GetProductById(Guid id)
    {
        return productRepository.GetById(id);
    }

    public ProductDto CreateProduct(CreateProductRequest request)
    {
        ProductDto? existingProduct = productRepository
            .GetByIdempotencyToken(request.IdempotencyToken);

        if (existingProduct != null)
        {
            throw new ConflictException(
              "Product already exists",
              errorCode: "product_already_exists");
        }

        var productToSave = new Product
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            IdempotencyToken = request.IdempotencyToken
        };

        return productRepository.Add(productToSave);
    }

    public bool DeleteProduct(Guid id)
    {
        return productRepository.Delete(id);
    }
}
