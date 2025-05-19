using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Models.ReceiptCategory;
using Recepify.BLL.Services;
using Recepify.Core.ResultPattern;

namespace Recepify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptCategoryController(ReceiptCategoryService receiptCategoryService) : ControllerBase
    {
        
        [HttpGet]
        public async Task<IResultBase> GetReceiptCategories()
        {
            return await receiptCategoryService.GetAllAsync();
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IResultBase> DeleteReceiptCategory(Guid id)
        {
            return await receiptCategoryService.DeleteAsync(id);
        }
        
        [HttpPost]
        public async Task<IResultBase> AddReceiptCategory([FromBody] AddReceiptCategoryDto receiptCategoryDto)
        {
            return await receiptCategoryService.AddAsync(receiptCategoryDto);
        }
    }
}
