using System.Collections.Concurrent;
using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public class OrderRepository : IOrderRepository
{
    private static readonly ConcurrentDictionary<int, Order> OrdersById = new();
    private static readonly ConcurrentDictionary<Guid, int> OrderIdByIdempotencyToken = new();
    private int _nextOrderId = OrdersById.Keys.Max() + 1;

    public Order Add(Order order)
    {
        bool isDuplicateOrder =
            OrdersById.ContainsKey(order.Id) ||
            (order.IdempotencyToken != Guid.Empty &&
             OrderIdByIdempotencyToken.ContainsKey(order.IdempotencyToken));

        if (isDuplicateOrder)
        {
            throw new InvalidOperationException("Order already exists");
        }

        var orderToSave = new Order
        {
            Id = _nextOrderId,
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

        // "Save" the order
        OrdersById[_nextOrderId] = orderToSave;
        OrderIdByIdempotencyToken[order.IdempotencyToken] = _nextOrderId;

        return orderToSave;
    }

    public Order? GetById(int id)
    {
        OrdersById.TryGetValue(id, out var order);
        return order;
    }

    public Order? GetByIdempotencyToken(Guid idempotencyToken)
    {
        if (idempotencyToken == Guid.Empty)
        {
            return null;
        }
        
        int orderId = OrderIdByIdempotencyToken
            .GetValueOrDefault(idempotencyToken, -1);

        if (orderId == -1)
        {
            return null;
        }

        if (!OrdersById.TryGetValue(orderId, out var order))
        {
            return null;
        }

        return order;      
    }
}
