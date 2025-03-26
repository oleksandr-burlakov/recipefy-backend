using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Models.ProductCategories;
using Recepify.BLL.Services;

namespace Recepify.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductCategoryController(ProductCategoryService productCategoryService) : ControllerBase 
{
    [HttpGet]
    public async Task<IActionResult> GetProductCategories()
    {
        var productCategories = await productCategoryService.GetProductCategoriesAsync();
        return Ok(productCategories);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductCategoryById(Guid id)
    {
        var productCategory = await productCategoryService.GetProductCategoryByIdAsync(id);
        return Ok(productCategory);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductCategory([FromBody] AddProductCategoryDto productCategoryDto)
    {
        var result = await productCategoryService.AddProductCategoryAsync(productCategoryDto);
        return Ok(result);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProductCategory(Guid id, [FromBody] UpdateProductCategoryDto productCategoryDto)
    {
        productCategoryDto.Id = id;
        var result = await productCategoryService.UpdateProductCategoryAsync(productCategoryDto);
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")] 
    public async Task<IActionResult> DeleteProductCategory(Guid id)
    {
        var result = await productCategoryService.DeleteProductCategoryAsync(id);
        return Ok(result);
    }
    
}