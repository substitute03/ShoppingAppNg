using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public class InMemoryProductRepository : IProductRepository
{
  private static readonly Lock Sync = new();
  private static readonly List<Product> Products =
  [
    new Product { Name = "Laptop", Price = 1299.99m },
    new Product { Name = "Headphones", Price = 149.99m },
    new Product { Name = "Keyboard", Price = 89.99m }
  ];

  public IReadOnlyList<Product> GetAll()
  {
    lock (Sync)
    {
      return Products.Select(Clone).ToList();
    }
  }

  public Product? GetById(Guid id)
  {
    lock (Sync)
    {
      var product = Products.FirstOrDefault(p => p.Id == id);
      return product is null ? null : Clone(product);
    }
  }

  public Product Add(Product product)
  {
    lock (Sync)
    {
      var toStore = Clone(product);
      Products.Add(toStore);
      return Clone(toStore);
    }
  }

  public bool Delete(Guid id)
  {
    lock (Sync)
    {
      return Products.RemoveAll(p => p.Id == id) > 0;
    }
  }

  private static Product Clone(Product product) =>
    new()
    {
      Id = product.Id,
      Name = product.Name,
      Price = product.Price
    };
}
