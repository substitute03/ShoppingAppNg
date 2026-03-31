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
            var toStore = Clone(order);
            Orders.Add(toStore);
            return Clone(toStore);
        }
    }

    public Order? GetById(Guid id)
    {
        lock (Sync)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            return order is null ? null : Clone(order);
        }
    }

    private static Order Clone(Order order) =>
      new()
      {
          Id = order.Id,
          CreatedAtUtc = order.CreatedAtUtc,
          PaymentSucceeded = order.PaymentSucceeded,
          Items = order.Items
          .Select(i => new OrderItem
          {
              ProductId = i.ProductId,
              Quantity = i.Quantity,
              UnitPrice = i.UnitPrice
          })
          .ToList()
      };
}
