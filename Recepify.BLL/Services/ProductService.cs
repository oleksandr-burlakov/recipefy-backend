using Microsoft.EntityFrameworkCore;
using Recepify.BLL.Models.Products;
using Recepify.DLL;
using Recepify.DLL.Entities;
using Recepify.Core.ResultPattern;
using System.Net;
using Microsoft.Extensions.Logging;

namespace Recepify.BLL.Services;

public class ProductService(RecepifyContext context, ILogger<ProductService> logger)
{
    public async Task<Result<ProductDto?>> GetProductByIdAsync(Guid id)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while loading product from database",
                ex.Message);
        }
    }

    public async Task<Result<ProductDto?>> AddProductAsync(AddProductDto productDto)
    {
        try
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
            return await GetProductByIdAsync(product.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while adding product to database",
                ex.Message);
        }
    }

    public async Task<Result<bool>> UpdateProductAsync(Product updateProduct)
    {
        try
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == updateProduct.Id);
            if (product is not null)
            {
                context.Products.Update(product);
                return await context.SaveChangesAsync() > 0;
            }

            return new Error((int)HttpStatusCode.BadRequest, "Product not found",
                "Product with id: " + updateProduct.Id + " not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while updating product in database",
                ex.Message);
        }
    }

    public async Task<Result<ICollection<ProductDto>>> GetAllProductsAsync()
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Product loading error", ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is not null)
            {
                context.Products.Remove(product);
                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while deleting product from database",
                ex.Message);
        }
    }
}