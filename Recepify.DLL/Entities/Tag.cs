namespace Recepify.DLL.Entities;

public class Tag : Entity
{
    public Tag()
    {
        Receipts = new HashSet<Recipe>();
    }
    public string Name { get; set; }
    public ICollection<Recipe> Receipts { get; set; }
}