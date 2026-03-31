using ShoppingAppApi.DataTransferObjects;
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
        Order? existingOrder = orderRepository
            .GetByIdempotencyToken(request.IdempotencyToken);

        if (existingOrder != null)
        {
            throw new InvalidOperationException("Order already exists");
        }

        Order orderToSave = MapCreateOrderRequestToOrder(request);

        return orderRepository.Add(orderToSave);
    }

    public Order? GetOrderById(int id)
    {
        return orderRepository.GetById(id);
    }

    private Order MapCreateOrderRequestToOrder(CreateOrderRequest request)
    {
        var order = new Order
        {
            Items = request.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = productRepository.GetById(item.ProductId)?.Price ??
                    throw new InvalidOperationException("Product not found")
            }).ToList(),
            PaymentSucceeded = !request.ForcePaymentFailure,
            IdempotencyToken = request.IdempotencyToken
        };
        
        return order;
    }
}
