namespace ShoppingAppApi.Models;

public class Order
{
    public int Id { get; init; } = 0;
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
    public required List<OrderItem> Items { get; init; }
    public required bool PaymentSucceeded { get; init; }
    public Guid IdempotencyToken { get; init; }
}
