using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recepify.DLL.Entities;

namespace Recepify.DLL.Configuration;

public class IngredientEntityTypeConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasKey(i => i.Id);
        builder.HasOne(i => i.Receipt).WithMany(r => r.Ingredients).HasForeignKey(i => i.ReceiptId);        
        builder.HasOne(i => i.Product).WithMany(p => p.Ingredients).HasForeignKey(i => i.ProductId);
    }
}