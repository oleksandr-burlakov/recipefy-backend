using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recepify.DLL.Entities;

namespace Recepify.DLL.Configuration;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.ProductCategoryId);
        builder.HasMany(p => p.Ingredients).WithOne(i => i.Product).HasForeignKey(i => i.ProductId);
        builder.Property(p => p.Name).HasMaxLength(255).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(1024);
    }
}