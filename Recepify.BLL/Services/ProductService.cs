using Microsoft.EntityFrameworkCore;
using Recepify.BLL.Models.Products;
using Recepify.DLL;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Services;

public class ProductService(RecepifyContext context)
{
    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        return await context.Products
            .Select(x => new ProductDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Image = x.Image,
                ProductCategoryId = x.ProductCategoryId,
                ProductCategoryName = x.Category.Name
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ProductDto?> AddProductAsync(AddProductDto productDto)
    {
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Image = productDto.Image,
            Name = productDto.Name,
            Description = productDto.Description,
            ProductCategoryId = productDto.ProductCategoryId
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        var productResult = await GetProductByIdAsync(product.Id);
        return productResult;
    }

    public async Task<bool> UpdateProductAsync(Product updateProduct)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == updateProduct.Id);
        if (product is not null)
        {
            context.Products.Update(product);
            return await context.SaveChangesAsync() > 0;
        }

        throw new ArgumentException("Product not found");
    }

    public async Task<ICollection<ProductDto>> GetAllProductsAsync()
    {
        var products = await context.Products
            .AsNoTracking()
            .Select(p => new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                ProductCategoryId = p.ProductCategoryId,
                ProductCategoryName = p.Category.Name
            })
            .ToListAsync();
        return products;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product is not null)
        {
            context.Products.Remove(product);
            return await context.SaveChangesAsync() > 0;
        }
        return false;
    }
}