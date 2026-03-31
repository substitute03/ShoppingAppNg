using Microsoft.AspNetCore.Mvc;
using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Services;

namespace ShoppingAppApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<ProductDto>> GetProducts()
    {
        return Ok(productService.GetProducts());
    }

    [HttpGet]
    public IActionResult GetProductById(Guid id)
    {
        var product = productService.GetProductById(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public ActionResult<ProductDto> CreateProduct([FromBody] ProductDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Product name is required.");
        }

        if (request.Price <= 0)
        {
            return BadRequest("Product price must be greater than zero.");
        }

        var createdProduct = productService.CreateProduct(request);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpDelete]
    public IActionResult DeleteProduct(Guid id)
    {
        return productService.DeleteProduct(id) ? NoContent() : NotFound();
    }
}
