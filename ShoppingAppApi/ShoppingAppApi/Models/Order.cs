namespace ShoppingAppApi.Models;

public class Order
{
  public Guid Id { get; init; } = Guid.NewGuid();
  public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
  public required List<OrderItem> Items { get; init; }
  public required bool PaymentSucceeded { get; init; }
}
