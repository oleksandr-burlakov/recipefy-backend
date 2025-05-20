using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recepify.BLL.Models.ReceiptCategory;
using Recepify.Core.ResultPattern;
using Recepify.DLL;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Services;

// TODO: Rename all 'Receipt' to 'Recipe' in controller, service, classes
public class RecipeCategoryService(ILogger<RecipeCategoryService> logger, RecepifyContext context)
{
    public async Task<Result<bool>> AddAsync(AddRecipeCategoryDto recipeCategoryDto)
    {
        try
        {
            var receiptCategory = new RecipeCategory()
            {
                Id = Guid.NewGuid(),
                Name = recipeCategoryDto.Name,
            };
            context.ReceiptCategories.Add(receiptCategory);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError,
                "Error while adding receipt category to database",
                ex.Message);
        }
        return true;
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var receiptCategory = await context.ReceiptCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (receiptCategory is not null)
            {
                context.ReceiptCategories.Remove(receiptCategory);
                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError,
                "Error while deleting receipt category from database",
                ex.Message);
        }
    }
    
    public async Task<Result<List<ReceiptCategoryDto>>> GetAllAsync()
    {
        try
        {
            return await context.ReceiptCategories
                .Select(x => new ReceiptCategoryDto()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError,
                "Error while loading receipt categories from database",
                ex.Message);
        }
    }
}