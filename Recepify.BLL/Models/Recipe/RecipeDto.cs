using Recepify.BLL.Models.Ingredients;
using Recepify.BLL.Models.ReceiptCategory;
using Recepify.BLL.Models.Tag;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Models.Receipt;

public class RecipeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int CookingTimeMinutes { get; set; }
    public ICollection<TagDto> Tags { get; set; }
    public ReceiptCategoryDto? RecipeCategory { get; set; }
    public ICollection<IngredientDto> Ingredients { get; set; }

    public RecipeDto()
    {
        
    }

    public RecipeDto(Recipe recipe)
    {
        CookingTimeMinutes = recipe.CookingTimeMinutes;
        PreparationTimeMinutes = recipe.PreparationTimeMinutes;
        Description = recipe.Description;
        Id = recipe.Id;
        Instructions = recipe.Instructions;
        Title = recipe.Title;
        Tags = recipe.Tags.Select(t => new TagDto()
        {
            Id = t.Id,
            Name = t.Name
        }).ToList();
        Ingredients = recipe.Ingredients.Select(i => new IngredientDto()
        {
            Id = i.Id,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Units = i.Units,
            ProductName = i.Product.Name
        }).ToList();
        RecipeCategory = new ReceiptCategoryDto()
        {
            Id = recipe.RecipeCategoryId,
            Name = recipe.Category.Name
        };
    }
}