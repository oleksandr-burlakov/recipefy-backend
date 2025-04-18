using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Models.Products;
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
        var products = await productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await productService.GetProductByIdAsync(id);
        return Ok(product);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] AddProductDto productRequest)
    {
        var result = await productService.AddProductAsync(productRequest);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await productService.DeleteAsync(id);
        return Ok(result);
    }
    
    // [HttpPut("{id:guid}")]
    // public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
    // {
    //     product.Id = id;
    //     var result = await productService.UpdateProduct(product);
    //     return Ok(result);
    // }
}
