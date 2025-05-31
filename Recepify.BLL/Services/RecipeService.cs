using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recepify.BLL.Models.Ingredients;
using Recepify.BLL.Models.Receipt;
using Recepify.BLL.Models.ReceiptCategory;
using Recepify.BLL.Models.Tag;
using Recepify.Core.ResultPattern;
using Recepify.DLL;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Services;

public class RecipeService(RecepifyContext context, ILogger<RecipeService> logger)
{
    public async Task<Result<RecipeDto>> AddAsync(AddRecipeDto recipeDto)
    {
        try
        {
            var tags = await context.Tags.Where(t => recipeDto.TagIds.Contains(t.Id)).ToListAsync();
            
            var recipe = new Recipe()
            {
                Id = Guid.NewGuid(),
                RecipeCategoryId = recipeDto.RecipeCategoryId,
                Title = recipeDto.Title,
                Description = recipeDto.Description,
                Instructions = recipeDto.Instructions,
                PreparationTimeMinutes = recipeDto.PreparationTimeMinutes,
                CookingTimeMinutes = recipeDto.CookingTimeMinutes,
                Tags = tags
            };
            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();
            
            var ingredients = recipeDto.Ingredients.Select(i => new Ingredient()
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Units = i.Units,
                RecipeId = recipe.Id 
            }).ToList();
            if (ingredients.Any())
            {
                context.Ingredients.AddRange(ingredients);
                await context.SaveChangesAsync();
            }

            return await this.GetByIdAsync(recipe.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while adding receipt to database",
                ex.Message);
        }
    }

    public async Task<Result<RecipeDto>> UpdateAsync(UpdateRecipeDto recipeDto)
    {
        try
        {
            var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id == recipeDto.Id);

            if (recipe is null)
            {
                return new Error(StatusCode: (int)HttpStatusCode.NotFound, Title: "Receipt not found", Description: "Receipt with id: " + recipeDto.Id + " not found");
            }

            if (recipeDto.TagIds.Any())
            {
                var tags = await context.Tags.Where(t => recipeDto.TagIds.Contains(t.Id)).ToListAsync();
                recipe.Tags = tags;
            }

            var oldIngredients = await context.Ingredients.Where(i => i.RecipeId == recipe.Id).ToListAsync();
            context.Ingredients.RemoveRange(oldIngredients);
            
            var ingredients = recipeDto.Ingredients.Select(i => new Ingredient()
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Units = i.Units,
                RecipeId = recipe.Id 
            }).ToList();
            if (ingredients.Any())
            {
                context.Ingredients.AddRange(ingredients);
                await context.SaveChangesAsync();
            }

            recipe.RecipeCategoryId = recipeDto.RecipeCategoryId;
            recipe.Title = recipeDto.Title;
            recipe.Description = recipeDto.Description;
            recipe.Instructions = recipeDto.Instructions;
            recipe.PreparationTimeMinutes = recipeDto.PreparationTimeMinutes;
            recipe.CookingTimeMinutes = recipeDto.CookingTimeMinutes;

            context.Recipes.Update(recipe);
            await context.SaveChangesAsync();
            return await this.GetByIdAsync(recipe.Id);
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
            var receipt = await context.Recipes.FirstOrDefaultAsync(x => x.Id == id);
            if (receipt is not null)
            {
                context.Recipes.Remove(receipt);
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

    public async Task<Result<List<RecipeDto>>> GetAllAsync()
    {
        try
        {
            return await context.Recipes
                .Select(x => new RecipeDto()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        Instructions = x.Instructions,
                        PreparationTimeMinutes = x.PreparationTimeMinutes,
                        CookingTimeMinutes = x.CookingTimeMinutes,
                        Tags = x.Tags.Select(t => new TagDto()
                        {
                            Name = t.Name,
                            Id = t.Id
                        }).ToList(),
                        Ingredients = x.Ingredients.Select(i => new IngredientDto()
                        {
                            Id = i.Id,
                            ProductId = i.ProductId,
                            Quantity = i.Quantity,
                            Units = i.Units,
                            ProductName = i.Product.Name
                        }).ToList(),
                        RecipeCategory = x.Category != null ? new ReceiptCategoryDto()
                        {
                            Id = x.Category.Id,
                            Name = x.Category.Name
                        } : null
                    }
                )
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while loading receipts from database",
                ex.Message);
        }
    }
    
    public async Task<Result<RecipeDto?>> GetByIdAsync(Guid id)
    {
        try
        {
            return await context.Recipes
                .Select(x => new RecipeDto()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        Instructions = x.Instructions,
                        PreparationTimeMinutes = x.PreparationTimeMinutes,
                        CookingTimeMinutes = x.CookingTimeMinutes,
                        Tags = x.Tags.Select(t => new TagDto()
                        {
                            Name = t.Name,
                            Id = t.Id
                        }).ToList(),
                        Ingredients = x.Ingredients.Select(i => new IngredientDto()
                        {
                            Id = i.Id,
                            RecipeId = i.RecipeId,
                            ProductId = i.ProductId,
                            Quantity = i.Quantity,
                            Units = i.Units,
                            ProductName = i.Product.Name
                        }).ToList(),
                        RecipeCategory = x.Category != null ? new ReceiptCategoryDto()
                        {
                            Id = x.Category.Id,
                            Name = x.Category.Name
                        } : null
                    }
                )
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while loading receipts from database",
                ex.Message);
        }
    }
}