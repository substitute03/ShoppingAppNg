using ShoppingAppApi.DataTransferObjects;

namespace ShoppingAppApi.Requests
{
    public class CreateOrderRequest
    {
        public List<OrderDto> Items { get; init; } = new List<OrderDto>();
        public bool ForcePaymentFailure { get; init; }
        public Guid IdempotencyToken { get; init;  }
    }
}
