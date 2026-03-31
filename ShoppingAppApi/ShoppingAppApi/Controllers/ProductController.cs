using Microsoft.AspNetCore.Mvc;
using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Requests;
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

    [HttpGet("{id:guid}")]
    public IActionResult GetProductById(Guid id)
    {
        var product = productService.GetProductById(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public ActionResult<ProductDto> CreateProduct([FromBody] CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Product name is required.");
        }

        if (request.Price <= 0)
        {
            return BadRequest("Product price must be greater than zero.");
        }

        try
        {
            var createdProduct = productService.CreateProduct(request);

            return CreatedAtAction(
                actionName: nameof(GetProductById),
                routeValues: new { id = createdProduct.Id },
                value: createdProduct);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteProduct(Guid id)
    {
        return productService.DeleteProduct(id) ? NoContent() : NotFound();
    }
}
