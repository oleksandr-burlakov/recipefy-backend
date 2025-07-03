namespace Recepify.DLL.Entities;

public class Product : Entity
{
    public Product()
    {
        Ingredients = new HashSet<Ingredient>();
    }
    public Guid ProductCategoryId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public ProductCategory Category { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
}