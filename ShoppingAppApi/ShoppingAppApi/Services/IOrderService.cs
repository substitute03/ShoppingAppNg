using ShoppingAppApi.Contracts;
using ShoppingAppApi.Models;

namespace ShoppingAppApi.Services;

public interface IOrderService
{
  Order CreateOrder(CreateOrderRequest request);
  Order? GetOrderById(Guid id);
}
