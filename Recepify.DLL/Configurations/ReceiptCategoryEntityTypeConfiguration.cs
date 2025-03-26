using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recepify.DLL.Entities;

namespace Recepify.DLL.Configuration;

public class ReceiptCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ReceiptCategory>
{
    public void Configure(EntityTypeBuilder<ReceiptCategory> builder)
    {
        builder.HasKey(rc => rc.Id);
        builder.Property(rc => rc.Name).HasMaxLength(255).IsRequired();
        builder.HasMany<Receipt>(rc => rc.Receipts).WithOne(r => r.Category).HasForeignKey(r => r.ReceiptCategoryId);
    }
}