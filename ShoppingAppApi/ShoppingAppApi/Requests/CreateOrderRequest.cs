using ShoppingAppApi.DataTransferObjects;

namespace ShoppingAppApi.Requests
{
    public class CreateOrderRequest
    {
        public List<OrderDto> Items { get; set; } = new List<OrderDto>();
        public bool ForcePaymentFailure { get; set; }
    }
}
