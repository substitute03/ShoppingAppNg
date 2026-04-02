using Microsoft.AspNetCore.Mvc;
using ShoppingAppApi.DataTransferObjects;
using ShoppingAppApi.Requests;
using ShoppingAppApi.Services;

namespace ShoppingAppApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(
    IProductService productService,
    IUserRoleService userRoleService) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<ProductDto>> GetProducts()
    {
        var products = productService
            .GetProducts();

        return Ok(products);
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
        if (!userRoleService.IsAdmin(HttpContext))
        {
            return Problem(
                statusCode: StatusCodes.Status403Forbidden,
                title: "Forbidden",
                detail: "Admin role is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: "Product name is required.");
        }

        if (request.Price <= 0)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: "Product price must be greater than zero.");
        }

        var createdProduct = productService.CreateProduct(request);

        return CreatedAtAction(
            actionName: nameof(GetProductById),
            routeValues: new { id = createdProduct.Id },
            value: createdProduct);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteProduct(Guid id)
    {
        if (!userRoleService.IsAdmin(HttpContext))
        {
            return Problem(
                statusCode: StatusCodes.Status403Forbidden,
                title: "Forbidden",
                detail: "Admin role is required.");
        }

        return productService.DeleteProduct(id) ? NoContent() : NotFound();
    }
}
