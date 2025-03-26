namespace Recepify.DLL.Entities;

public class Receipt : Entity
{
    public Receipt()
    {
        Tags = new HashSet<Tag>();
        Ingredients = new HashSet<Ingredient>();
    }

    public Guid ReceiptCategoryId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int CookingTimeMinutes { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public ReceiptCategory? Category { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
}