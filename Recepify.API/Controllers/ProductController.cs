using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Models.Products;
using Recepify.BLL.Services;
using Recepify.Core.ResultPattern;

namespace Recepify.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IResultBase> GetProducts()
    {
        return await productService.GetAllProductsAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<IResultBase> GetProductById(Guid id)
    {
        return await productService.GetProductByIdAsync(id);
    }

    [HttpPost]
    public async Task<IResultBase> AddProduct([FromBody] AddProductDto productRequest)
    {
        return await productService.AddProductAsync(productRequest);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IResultBase> DeleteProduct(Guid id)
    {
        return await productService.DeleteAsync(id);
    }

    // [HttpPut("{id:guid}")]
    // public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
    // {
    //     product.Id = id;
    //     var result = await productService.UpdateProduct(product);
    //     return Ok(result);
    // }
}
