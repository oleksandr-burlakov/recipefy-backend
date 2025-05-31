using Recepify.DLL.Enums;

namespace Recepify.BLL.Models.Ingredients;

public class AddIngredientDto
{
    public Guid ProductId { get; set; }
    public MeasurementUnits Units { get; set; }
    public double Quantity { get; set; }
}