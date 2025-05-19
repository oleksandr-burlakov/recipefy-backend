
namespace Recepify.BLL.Models.Receipt;

public class AddReceiptDto
{
    public Guid ReceiptCategoryId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Instructions { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int CookingTimeMinutes { get; set; }
    public ICollection<Guid> TagIds { get; set; } = new List<Guid>();
    public ICollection<Guid> IngredientIds { get; set; } = new List<Guid>();

}
