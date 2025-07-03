using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recepify.DLL.Entities;

namespace Recepify.DLL.Configuration;

public class ReceiptEntityTypeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.Category).WithMany(rc => rc.Recipes).HasForeignKey(r => r.RecipeCategoryId);
        builder.HasMany(r => r.Ingredients).WithOne(i => i.Recipe).HasForeignKey(i => i.RecipeId);
        builder.Property(r => r.Title).HasMaxLength(255).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(2048).IsRequired();
        builder.Property(r => r.Instructions).IsRequired();
    }
}