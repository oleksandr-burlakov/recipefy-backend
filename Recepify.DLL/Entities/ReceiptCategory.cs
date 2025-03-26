namespace Recepify.DLL.Entities;

public class ReceiptCategory : Entity
{
    public ReceiptCategory()
    {
        Receipts = new HashSet<Receipt>();
    }
    
    public string Name { get; set; }
    public ICollection<Receipt> Receipts { get; set; }
}