using Microsoft.AspNetCore.Mvc;

namespace ShoppingAppApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
  [HttpPost]
  public IActionResult CreateOrder()
  {
    return Ok();
  }

  [HttpGet("{id:guid}")]
  public IActionResult GetOrderById(Guid id)
  {
    return Ok();
  }
}
