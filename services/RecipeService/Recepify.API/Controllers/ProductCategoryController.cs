using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Models.ProductCategories;
using Recepify.BLL.Services;
using Recepify.Core.ResultPattern;

namespace Recepify.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductCategoryController(ProductCategoryService productCategoryService) : ControllerBase 
{
    [HttpGet]
    public async Task<IResultBase> GetProductCategories()
    {
        return await productCategoryService.GetProductCategoriesAsync();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IResultBase> GetProductCategoryById(Guid id)
    {
        return await productCategoryService.GetProductCategoryByIdAsync(id);
    }

    [HttpPost]
    public async Task<IResultBase> AddProductCategory([FromBody] AddProductCategoryDto productCategoryDto)
    {
        return await productCategoryService.AddProductCategoryAsync(productCategoryDto);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IResultBase> UpdateProductCategory(Guid id, [FromBody] UpdateProductCategoryDto productCategoryDto)
    {
        productCategoryDto.Id = id;
        return await productCategoryService.UpdateProductCategoryAsync(productCategoryDto);
    }
    
    [HttpDelete("{id:guid}")] 
    public async Task<IResultBase> DeleteProductCategory(Guid id)
    {
        return await productCategoryService.DeleteProductCategoryAsync(id);
    }
    
}