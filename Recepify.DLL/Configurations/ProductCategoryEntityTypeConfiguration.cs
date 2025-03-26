using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recepify.DLL.Entities;

namespace Recepify.DLL.Configuration;

public class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(pc => pc.Id);
        builder.Property(pc => pc.Name).HasMaxLength(255).IsRequired();
        builder.HasMany<Product>(pc => pc.Products).WithOne(p => p.Category).HasForeignKey(p => p.ProductCategoryId);
    }
}