using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public class OrderRepository : IOrderRepository
{
    private static readonly Lock Sync = new();
    private static readonly List<Order> Orders = [];

    public Order Add(Order order)
    {
        lock (Sync)
        {
            if (Orders.Any(o => o.IdempotencyToken == order.IdempotencyToken))
            {
                return order;
            }

            Orders.Add(order);
            return Clone(order);
        }
    }

    public Order? GetById(int id)
    {
        lock (Sync)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            return order is null ? null : Clone(order);
        }
    }

    public Order? GetByIdempotencyToken(Guid idempotencyToken)
    {
        lock (Sync)
        {
            var order = Orders.FirstOrDefault(o =>
                o.IdempotencyToken == idempotencyToken);

            return order is null ? null : Clone(order);
        }
    }

    private static Order Clone(Order order)
    {
        return new Order
        {
            Id = order.Id,
            CreatedAtUtc = order.CreatedAtUtc,
            PaymentSucceeded = order.PaymentSucceeded,
            Items = order.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}
