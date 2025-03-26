using Microsoft.EntityFrameworkCore;
using Recepify.BLL.Models.ProductCategories;
using Recepify.DLL;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Services;

public class ProductCategoryService(RecepifyContext context)
{
    public async Task<bool> DeleteProductCategoryAsync(Guid id)
    {
        var productCategory = await context.ProductCategories
            .FirstOrDefaultAsync(pc => pc.Id == id);
        if (productCategory != null)
        {
            context.ProductCategories.Remove(productCategory);
            return await context.SaveChangesAsync() > 0;
        }
        return false;
    }
    public async Task<bool> AddProductCategoryAsync(AddProductCategoryDto addProductCategoryDto)
    {
        var productCategory = new ProductCategory()
        {
            Id = Guid.NewGuid(),
            Name = addProductCategoryDto.Name,
        };
        context.ProductCategories.Add(productCategory);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateProductCategoryAsync(UpdateProductCategoryDto productCategoryDto)
    {
        var productCategory = await context.ProductCategories
            .FirstOrDefaultAsync(pc => pc.Id == productCategoryDto.Id);
        if (productCategory != null)
        {
            productCategory.Name = productCategoryDto.Name;
            context.ProductCategories.Update(productCategory);
            return await context.SaveChangesAsync() > 0;
        }

        return false;
    }

    public async Task<ICollection<ProductCategoryDto>> GetProductCategoriesAsync()
    {
        return await context.ProductCategories
            .AsNoTracking()
            .Select(pc => new ProductCategoryDto()
            {
                Id = pc.Id,
                Name = pc.Name
            })
            .ToListAsync();
    }

    public async Task<ProductCategoryDto?> GetProductCategoryByIdAsync(Guid id)
    {
        return await context.ProductCategories
            .AsNoTracking()
            .Select(pc => new ProductCategoryDto()
            {
                Id = pc.Id,
                Name = pc.Name
            })
            .FirstOrDefaultAsync(pc => pc.Id == id);
    }
}