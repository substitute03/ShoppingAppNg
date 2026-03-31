using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public interface IOrderRepository
{
    Order Add(Order order);
    Order? GetById(Guid id);
}
