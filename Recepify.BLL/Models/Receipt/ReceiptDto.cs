using Recepify.BLL.Models.ReceiptCategory;
using Recepify.BLL.Models.Tag;

namespace Recepify.BLL.Models.Receipt;

public class ReceiptDto
{
    public Guid RecipeCategoryId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int CookingTimeMinutes { get; set; }
    public ICollection<TagDto> Tags { get; set; }
    public ReceiptCategoryDto ReceiptCategory { get; set; }
}