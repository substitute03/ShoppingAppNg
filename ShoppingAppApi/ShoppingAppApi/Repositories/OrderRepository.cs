using System.Collections.Concurrent;
using ShoppingAppApi.Models;
using ShoppingAppApi.Exceptions;

namespace ShoppingAppApi.Repositories;

public class OrderRepository : IOrderRepository
{
    private static readonly ConcurrentDictionary<Guid, Order> OrdersById = new();
    private static readonly ConcurrentDictionary<Guid, Guid> OrderIdByIdempotencyToken = new();

    public Order Add(Order order)
    {
        bool isDuplicateOrder =
            (order.Id != Guid.Empty && OrdersById.ContainsKey(order.Id)) ||
            (order.IdempotencyToken != Guid.Empty &&
             OrderIdByIdempotencyToken.ContainsKey(order.IdempotencyToken));

        if (isDuplicateOrder)
        {
            throw new ConflictException(
              "Order already exists",
              errorCode: "order_already_exists");
        }

        var newId = Guid.NewGuid();
        var orderToSave = new Order
        {
            Id = newId,
            CreatedAtUtc = order.CreatedAtUtc,
            PaymentSucceeded = order.PaymentSucceeded,
            IdempotencyToken = order.IdempotencyToken,
            Items = order.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        OrdersById[newId] = orderToSave;

        if (order.IdempotencyToken != Guid.Empty)
        {
            OrderIdByIdempotencyToken[order.IdempotencyToken] = newId;
        }

        return orderToSave;
    }

    public Order? GetById(Guid id)
    {
        return OrdersById.TryGetValue(id, out var order) ? order : null;
    }

    public Order? GetByIdempotencyToken(Guid idempotencyToken)
    {
        if (idempotencyToken == Guid.Empty)
        {
            return null;
        }

        if (!OrderIdByIdempotencyToken.TryGetValue(idempotencyToken, out var orderId))
        {
            return null;
        }

        return OrdersById.TryGetValue(orderId, out var order) ? order : null;
    }
}
