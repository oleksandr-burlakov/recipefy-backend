using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Models.Receipt;
using Recepify.BLL.Services;
using Recepify.Core.ResultPattern;

namespace Recepify.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RecipeController(RecipeService recipeService) : ControllerBase
{

    [HttpGet]
    public async Task<IResultBase> GetAllAsync()
    {
        return await recipeService.GetAllAsync();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IResultBase> GetReceiptById(Guid id)
    {
        return await recipeService.GetByIdAsync(id);
    }
    
    [HttpPost]
    public async Task<IResultBase> AddReceipt([FromBody] AddReceiptDto receiptRequest)
    {
        return await recipeService.AddAsync(receiptRequest);
    }
    
    [HttpPut]
    public async Task<IResultBase> UpdateReceipt([FromBody] UpdateReceiptDto receiptRequest)
    {
        return await recipeService.UpdateAsync(receiptRequest);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IResultBase> DeleteReceipt(Guid id)
    {
        return await recipeService.DeleteAsync(id);
    }

}
