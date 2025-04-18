namespace Recepify.BLL.Models.Products;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public Guid ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; }
}