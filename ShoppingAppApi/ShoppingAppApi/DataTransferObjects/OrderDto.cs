namespace ShoppingAppApi.DataTransferObjects;

public class OrderDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}
