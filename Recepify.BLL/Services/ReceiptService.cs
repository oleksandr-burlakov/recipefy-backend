using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recepify.BLL.Models.Receipt;
using Recepify.Core.ResultPattern;
using Recepify.DLL;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Services;

public class ReceiptService(RecepifyContext context, ILogger<ReceiptService> logger)
{
    public async Task<Result<bool>> AddAsync(AddReceiptDto receiptDto)
    {
        try
        {
            var tags = await context.Tags.Where(t => receiptDto.TagIds.Contains(t.Id)).ToListAsync();
            var ingredients =
                await context.Ingredients.Where(i => receiptDto.IngredientIds.Contains(i.Id)).ToListAsync();
            var receipt = new Receipt()
            {
                Id = Guid.NewGuid(),
                ReceiptCategoryId = receiptDto.ReceiptCategoryId,
                Title = receiptDto.Title,
                Description = receiptDto.Description,
                Instructions = receiptDto.Instructions,
                PreparationTimeMinutes = receiptDto.PreparationTimeMinutes,
                CookingTimeMinutes = receiptDto.CookingTimeMinutes,
                Tags = tags,
                Ingredients = ingredients
            };
            context.Receipts.Add(receipt);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while adding receipt to database",
                ex.Message);
        }
    }

    public async Task<Result<bool>> UpdateAsync(UpdateReceiptDto receiptDto)
    {
        try
        {
            var receipt = await context.Receipts.FirstOrDefaultAsync(x => x.Id == receiptDto.Id);

            if (receipt is null)
            {
                return false;
            }

            if (receiptDto.TagIds.Any())
            {
                var tags = await context.Tags.Where(t => receiptDto.TagIds.Contains(t.Id)).ToListAsync();
                receipt.Tags = tags;
            }

            if (receiptDto.IngredientIds.Any())
            {
                var ingredients =
                    await context.Ingredients.Where(i => receiptDto.IngredientIds.Contains(i.Id)).ToListAsync();
                receipt.Ingredients = ingredients;
            }

            receipt.ReceiptCategoryId = receiptDto.ReceiptCategoryId;
            receipt.Title = receiptDto.Title;
            receipt.Description = receiptDto.Description;
            receipt.Instructions = receiptDto.Instructions;
            receipt.PreparationTimeMinutes = receiptDto.PreparationTimeMinutes;
            receipt.CookingTimeMinutes = receiptDto.CookingTimeMinutes;

            context.Receipts.Update(receipt);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while updating receipt in database",
                ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var receipt = await context.Receipts.FirstOrDefaultAsync(x => x.Id == id);
            if (receipt is not null)
            {
                context.Receipts.Remove(receipt);
                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while deleting receipt from database",
                ex.Message);
        }
    }

    public async Task<Result<List<Receipt>>> GetAllAsync()
    {
        try
        {
            return await context.Receipts.ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while loading receipts from database",
                ex.Message);
        }
    }
    
    public async Task<Result<Receipt?>> GetByIdAsync(Guid id)
    {
        try
        {
            return await context.Receipts.FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while loading receipts from database",
                ex.Message);
        }
    }
}