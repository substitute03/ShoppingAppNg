using Microsoft.AspNetCore.Mvc;

namespace ShoppingAppApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
  [HttpGet]
  public IActionResult GetProducts()
  {
    return Ok();
  }

  [HttpGet("{id:guid}")]
  public IActionResult GetProductById(Guid id)
  {
    return Ok();
  }

  [HttpPost]
  public IActionResult CreateProduct()
  {
    return Created(string.Empty, null);
  }

  [HttpDelete("{id:guid}")]
  public IActionResult DeleteProduct(Guid id)
  {
    return NoContent();
  }
}
