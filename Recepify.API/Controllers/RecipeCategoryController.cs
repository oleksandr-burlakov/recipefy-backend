using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Models.ReceiptCategory;
using Recepify.BLL.Services;
using Recepify.Core.ResultPattern;

namespace Recepify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeCategoryController(RecipeCategoryService recipeCategoryService) : ControllerBase
    {
        
        [HttpGet]
        public async Task<IResultBase> GetReceiptCategories()
        {
            return await recipeCategoryService.GetAllAsync();
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IResultBase> DeleteReceiptCategory(Guid id)
        {
            return await recipeCategoryService.DeleteAsync(id);
        }
        
        [HttpPost]
        public async Task<IResultBase> AddReceiptCategory([FromBody] AddRecipeCategoryDto recipeCategoryDto)
        {
            return await recipeCategoryService.AddAsync(recipeCategoryDto);
        }
    }
}
