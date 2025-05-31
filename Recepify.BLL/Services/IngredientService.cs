using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recepify.BLL.Models.Ingredients;
using Recepify.Core.ResultPattern;
using Recepify.DLL;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Services;

public class IngredientService(RecepifyContext context, ILogger<IngredientService> logger)
{
    public async Task<Result<IngredientDto?>> GetByIdAsync(Guid id)
    {
        return await context.Ingredients.Where(x => x.Id == id).Select(x => new IngredientDto()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                RecipeId = x.RecipeId,
                ProductName = x.Product.Name,
                Quantity = x.Quantity,
                Units = x.Units,
            })
            .FirstOrDefaultAsync(
            );
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var ingredient = await context.Ingredients.FirstOrDefaultAsync(x => x.Id == id);
        if (ingredient is not null)
        {
            context.Ingredients.Remove(ingredient);
            return await context.SaveChangesAsync() > 0;
        }

        return false;
    }

    public async Task<Result<ICollection<IngredientDto>>> GetByRecipeIdAsync(Guid recipeId)
    {
        return await context.Ingredients.Where(x => x.RecipeId == recipeId).Select(x => new IngredientDto()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                RecipeId = x.RecipeId,
                ProductName = x.Product.Name,
                Quantity = x.Quantity,
                Units = x.Units,
            })
            .ToListAsync();
    }
}