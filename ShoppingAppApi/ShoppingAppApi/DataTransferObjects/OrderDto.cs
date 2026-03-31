namespace ShoppingAppApi.Contracts;

public class CreateOrderItemRequest
{
  public Guid ProductId { get; init; }
  public int Quantity { get; init; }
}
