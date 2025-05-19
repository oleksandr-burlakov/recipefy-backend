using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recepify.BLL.Models.ProductCategories;
using Recepify.Core.ResultPattern;
using Recepify.DLL;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Services;

public class ProductCategoryService(RecepifyContext context, ILogger<ProductCategoryService> logger)
{
    public async Task<Result<bool>> DeleteProductCategoryAsync(Guid id)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError,
                "Error while deleting product category from database",
                ex.Message);
        }
    }

    public async Task<Result<bool>> AddProductCategoryAsync(AddProductCategoryDto addProductCategoryDto)
    {
        try
        {
            var productCategory = new ProductCategory()
            {
                Id = Guid.NewGuid(),
                Name = addProductCategoryDto.Name,
            };
            context.ProductCategories.Add(productCategory);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError,
                "Error while adding product category to database",
                ex.Message);
        }
    }

    public async Task<Result<bool>> UpdateProductCategoryAsync(UpdateProductCategoryDto productCategoryDto)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError,
                "Error while updating product category in database",
                ex.Message);
        }
    }

    public async Task<Result<ICollection<ProductCategoryDto>>> GetProductCategoriesAsync()
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError,
                "Error while loading product categories from database",
                ex.Message);
        }
    }

    public async Task<Result<ProductCategoryDto?>> GetProductCategoryByIdAsync(Guid id)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError,
                "Error while loading product category from database",
                ex.Message);
        }
    }
}