using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recepify.DLL.Entities;

namespace Recepify.DLL.Configuration;

public class ReceiptCategoryEntityTypeConfiguration : IEntityTypeConfiguration<RecipeCategory>
{
    public void Configure(EntityTypeBuilder<RecipeCategory> builder)
    {
        builder.HasKey(rc => rc.Id);
        builder.Property(rc => rc.Name).HasMaxLength(255).IsRequired();
        builder.HasMany<Recipe>(rc => rc.Recipes).WithOne(r => r.Category).HasForeignKey(r => r.RecipeCategoryId);
    }
}