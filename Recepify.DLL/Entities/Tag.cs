namespace Recepify.DLL.Entities;

public class Tag : Entity
{
    public Tag()
    {
        Receipts = new HashSet<Receipt>();
    }
    public string Name { get; set; }
    public ICollection<Receipt> Receipts { get; set; }
}