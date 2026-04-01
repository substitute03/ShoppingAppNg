namespace ShoppingAppApi.DataTransferObjects;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
}
