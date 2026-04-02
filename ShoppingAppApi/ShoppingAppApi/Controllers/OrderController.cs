using Microsoft.AspNetCore.Mvc;
using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Models;
using ShoppingAppApi.Requests;
using ShoppingAppApi.Services;

namespace ShoppingAppApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public ActionResult<OrderDto> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (request.Items.Count == 0)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: "Order must include at least one item.");
        }

        if (request.Items.Any(i => i.Quantity <= 0))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: "Order item quantity must be greater than zero.");
        }

        Order order = orderService.CreateOrder(request);

        return CreatedAtAction(
            actionName: nameof(GetOrderById),
            routeValues: new { id = order.Id },
            value: order);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Order> GetOrderById(Guid id)
    {
        Order? order = orderService.GetOrderById(id);
        return order is null ? NotFound() : Ok(order);
    }
}
