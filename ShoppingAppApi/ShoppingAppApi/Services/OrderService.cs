using ShoppingAppApi.Models;
using ShoppingAppApi.Repositories;
using ShoppingAppApi.Requests;

namespace ShoppingAppApi.Services;

public class OrderService(
  IOrderRepository orderRepository,
  IProductRepository productRepository) : IOrderService
{
    public Order CreateOrder(CreateOrderRequest request)
    {
        var orderItems = request.Items
          .Select(item =>
          {
              var product = productRepository.GetById(item.ProductId);
              if (product is null)
              {
                  throw new InvalidOperationException($"Product '{item.ProductId}' was not found.");
              }

              return new OrderItem
              {
                  ProductId = item.ProductId,
                  Quantity = item.Quantity,
                  UnitPrice = product.Price
              };
          })
          .ToList();

        var order = new Order
        {
            Items = orderItems,
            PaymentSucceeded = !request.ForcePaymentFailure
        };

        return orderRepository.Add(order);
    }

    public Order? GetOrderById(Guid id)
    {
        return orderRepository.GetById(id);
    }
}
