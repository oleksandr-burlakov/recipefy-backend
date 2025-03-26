using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Services;
using Recepify.DLL.Entities;

namespace Recepify.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await productService.GetProducts();
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await productService.GetProductById(id);
        return Ok(product);
    }
    
    // [HttpPost]
    // public async Task<IActionResult> AddProduct([FromBody] Product product)
    // {
    //     var result = await productService.AddProduct(product);
    //     return Ok(result);
    // }
    //
    // [HttpPut("{id:guid}")]
    // public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
    // {
    //     product.Id = id;
    //     var result = await productService.UpdateProduct(product);
    //     return Ok(result);
    // }
}
