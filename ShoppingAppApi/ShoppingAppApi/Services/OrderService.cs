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
        Order existingOrder = orderRepository
            .GetByIdempotencyToken(request.IdempotencyToken);

        if (existingOrder != null)
        {
            throw new InvalidOperationException("Order already exists");
        }

        var orderItems = request.Items.Select(item =>
        {
            Product? product =
            productRepository.GetById(item.ProductId);

            if (product == null)
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

    public Order? GetOrderById(int id)
    {
        return orderRepository.GetById(id);
    }

    private Order MapCreateOrderRequestToOrder(CreateOrderRequest request){
        var order = new Order
        {
            Items = request.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Price
            }).ToList(),
            PaymentSucceeded = !request.ForcePaymentFailure
        };
        
        return order;
    }
}
