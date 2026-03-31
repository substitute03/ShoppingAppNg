namespace ShoppingAppApi.Contracts;

public class CreateOrderRequest
{
  public List<OrderDto> Items { get; init; } = [];
  public bool ForcePaymentFailure { get; init; }
}
