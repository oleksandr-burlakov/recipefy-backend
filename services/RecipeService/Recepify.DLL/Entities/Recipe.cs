namespace Recepify.DLL.Entities;

public class Recipe : Entity
{
    public Recipe()
    {
        Tags = new HashSet<Tag>();
        Ingredients = new HashSet<Ingredient>();
    }

    public Guid RecipeCategoryId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int CookingTimeMinutes { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public RecipeCategory? Category { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
}