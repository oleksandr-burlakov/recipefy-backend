
using Recepify.BLL.Models.Ingredients;

namespace Recepify.BLL.Models.Receipt;

public class UpdateRecipeDto
{
    public Guid Id { get; set; }
    public Guid RecipeCategoryId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Instructions { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int CookingTimeMinutes { get; set; }
    public ICollection<Guid> TagIds { get; set; } = new List<Guid>();
    public ICollection<AddIngredientDto> Ingredients { get; set; } = new List<AddIngredientDto>();
}

