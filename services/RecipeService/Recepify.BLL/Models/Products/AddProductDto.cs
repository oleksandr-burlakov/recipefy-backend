namespace Recepify.BLL.Models.Products;

public class AddProductDto
{
    public string? Image { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid ProductCategoryId { get; set; }
}