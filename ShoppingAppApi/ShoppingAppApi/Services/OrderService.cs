using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Models;
using ShoppingAppApi.Repositories;
using ShoppingAppApi.Requests;
using ShoppingAppApi.Exceptions;

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
            throw new ConflictException("Order already exists", errorCode: "order_already_exists");
        }

        ProcessPayment(request.ForcePaymentFailure);

        Order orderToSave = MapCreateOrderRequestToOrder(request);

        return orderRepository.Add(orderToSave);
    }

    public Order? GetOrderById(Guid id)
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
                    throw new ResourceNotFoundException(
                      clientMessage: "Product not found",
                      errorCode: "product_not_found")
            }).ToList(),
            PaymentSucceeded = !request.ForcePaymentFailure,
            IdempotencyToken = request.IdempotencyToken
        };
        
        return order;
    }

    private void ProcessPayment(bool forcePaymentFailure)
    {
        if (forcePaymentFailure)
        {
            throw new PaymentFailedException();
        }
    }
}
