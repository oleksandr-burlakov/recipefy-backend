using Recepify.DLL.Enums;

namespace Recepify.DLL.Entities;

public class Ingredient : Entity
{
    public Guid ReceiptId { get; set; }
    public Guid ProductId { get; set; }
    public MeasurementUnits Units { get; set; }
    public Product Product { get; set; }
    public double Quantity { get; set; }
    public Receipt Receipt { get; set; }
}