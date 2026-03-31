using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Models;
using ShoppingAppApi.Repositories;

namespace ShoppingAppApi.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public IReadOnlyList<Product> GetProducts()
    {
        return productRepository.GetAll();
    }

    public Product? GetProductById(Guid id)
    {
        return productRepository.GetById(id);
    }

    public Product CreateProduct(ProductDto request)
    {
        var product = new Product
        {
            Name = request.Name.Trim(),
            Price = request.Price
        };

        return productRepository.Add(product);
    }

    public bool DeleteProduct(Guid id)
    {
        return productRepository.Delete(id);
    }
}
