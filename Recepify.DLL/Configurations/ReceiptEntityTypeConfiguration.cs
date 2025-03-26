using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recepify.DLL.Entities;

namespace Recepify.DLL.Configuration;

public class ReceiptEntityTypeConfiguration : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.Category).WithMany(rc => rc.Receipts).HasForeignKey(r => r.ReceiptCategoryId);
        builder.HasMany(r => r.Tags).WithMany(t => t.Receipts);
        builder.HasMany(r => r.Ingredients).WithOne(i => i.Receipt).HasForeignKey(i => i.ReceiptId);
        builder.Property(r => r.Title).HasMaxLength(255).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(2048).IsRequired();
        builder.Property(r => r.Instructions).IsRequired();
    }
}