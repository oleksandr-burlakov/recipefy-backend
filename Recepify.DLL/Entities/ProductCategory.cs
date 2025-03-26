namespace Recepify.DLL.Entities;

public class ProductCategory : Entity
{
    public ProductCategory()
    {
        Products = new HashSet<Product>();
    }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}