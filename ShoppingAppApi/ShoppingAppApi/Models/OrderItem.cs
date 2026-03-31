namespace ShoppingAppApi.Models;

public class OrderItem
{
  public Guid ProductId { get; init; }
  public int Quantity { get; init; }
  public decimal UnitPrice { get; init; }
}
