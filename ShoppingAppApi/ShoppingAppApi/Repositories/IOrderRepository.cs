using ShoppingAppApi.Models;

namespace ShoppingAppApi.Repositories;

public interface IOrderRepository
{
    Order Add(Order order);
    Order? GetById(int id);
    Order? GetByIdempotencyToken(Guid idempotencyToken);
}
