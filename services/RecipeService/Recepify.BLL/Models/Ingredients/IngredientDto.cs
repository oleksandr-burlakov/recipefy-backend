using Recepify.DLL.Enums;

namespace Recepify.BLL.Models.Ingredients;

public class IngredientDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid RecipeId { get; set; }
    public string ProductName { get; set; }
    public double Quantity { get; set; }
    public MeasurementUnits Units { get; set; }
}