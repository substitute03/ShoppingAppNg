namespace ShoppingAppApi.Models;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid IdempotencyToken { get; init; }
}
