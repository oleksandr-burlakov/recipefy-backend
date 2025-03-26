using Microsoft.EntityFrameworkCore;
using Recepify.BLL.Models.Products;
using Recepify.DLL;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Services;

public class ProductService(RecepifyContext context)
{

    public async Task<ICollection<Product>> GetProducts()
    {
        return await context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        return await context.Products
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddProduct(AddProductDto productDto)
    {
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            

        };
        context.Products.Add(product);
        return await context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> UpdateProduct(Product product)
    {
        context.Products.Update(product);
        return await context.SaveChangesAsync() > 0;
    }
}