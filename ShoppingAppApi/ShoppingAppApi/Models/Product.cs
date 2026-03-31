namespace ShoppingAppApi.Models;

public class Product
{
    public int Id { get; init; } = 0;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
