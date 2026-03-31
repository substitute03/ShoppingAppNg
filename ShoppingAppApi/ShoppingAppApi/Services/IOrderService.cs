using ShoppingAppApi.Models;
using ShoppingAppApi.Requests;

namespace ShoppingAppApi.Services;

public interface IOrderService
{
    Order CreateOrder(CreateOrderRequest request);
    Order? GetOrderById(Guid id);
}
