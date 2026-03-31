using Microsoft.AspNetCore.Mvc;
using ShoppingAppApi.Contracts;
using ShoppingAppApi.Models;
using ShoppingAppApi.Services;

namespace ShoppingAppApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public ActionResult<Order> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (request.Items.Count == 0)
        {
            return BadRequest("Order must include at least one item.");
        }

        if (request.Items.Any(i => i.Quantity <= 0))
        {
            return BadRequest("Order item quantity must be greater than zero.");
        }

        try
        {
            Order order = orderService.CreateOrder(request);

            return CreatedAtAction(nameof(GetOrderById),
                new { id = order.Id }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public ActionResult<Order> GetOrderById(Guid id)
    {
        Order? order = orderService.GetOrderById(id);
        return order is null ? NotFound() : Ok(order);
    }
}
