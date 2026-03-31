using ShoppingAppApi.Models;
using ShoppingAppApi.Repositories;
using ShoppingAppApi.Requests;

namespace ShoppingAppApi.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public IReadOnlyList<Product> GetProducts()
    {
        return productRepository.GetAll();
    }

    public Product? GetProductById(int id)
    {
        return productRepository.GetById(id);
    }

    public Product CreateProduct(CreateProductRequest request)
    {
        Product? existingProduct = productRepository
            .GetByIdempotencyToken(request.IdempotencyToken);

        if (existingProduct != null)
        {
            throw new InvalidOperationException("Product already exists");
        }

        var productToSave = new Product
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            IdempotencyToken = request.IdempotencyToken
        };

        return productRepository.Add(productToSave);
    }

    public bool DeleteProduct(int id)
    {
        return productRepository.Delete(id);
    }
}
