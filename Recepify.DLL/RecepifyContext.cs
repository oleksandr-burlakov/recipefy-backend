using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recepify.DLL.Entities;

namespace Recepify.DLL;

public class RecepifyContext : IdentityDbContext<User, Role, Guid>
{

    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<ReceiptCategory> ReceiptCategories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    public RecepifyContext(DbContextOptions<RecepifyContext> options) : base(options)
    {
        
    }

    public RecepifyContext()
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecepifyContext).Assembly);
    }
}
