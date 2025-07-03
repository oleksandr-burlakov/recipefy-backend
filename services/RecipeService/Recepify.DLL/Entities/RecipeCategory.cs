namespace Recepify.DLL.Entities;

public class RecipeCategory : Entity
{
    public RecipeCategory()
    {
        Recipes = new HashSet<Recipe>();
    }
    
    public string Name { get; set; }
    public ICollection<Recipe> Recipes { get; set; }
}