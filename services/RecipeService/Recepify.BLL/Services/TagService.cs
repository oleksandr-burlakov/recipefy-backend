using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recepify.BLL.Models.Tag;
using Recepify.Core.ResultPattern;
using Recepify.DLL;
using Recepify.DLL.Entities;

namespace Recepify.BLL.Services;

public class TagService(RecepifyContext context, ILogger<TagService> logger)
{
    public async Task<Result<TagDto?>> AddAsync(AddTagDto tagDto)
    {
        try
        {
            var existingTag = await context.Tags.FirstOrDefaultAsync(x => x.Name.Equals(tagDto.Name));
            if (existingTag is not null)
            {
                return new TagDto()
                {
                    Name = existingTag.Name,
                    Id = existingTag.Id
                };
            }

            var tag = new Tag()
            {
                Id = Guid.NewGuid(),
                Name = tagDto.Name
            };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();
            return await GetByIdAsync(tag.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while adding receipt to database",
                ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag is not null)
            {
                context.Tags.Remove(tag);
                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while deleting receipt from database",
                ex.Message);
        }
    }

    public async Task<Result<List<TagDto>>> GetAllAsync()
    {
        try
        {
            return await context.Tags.Select(x => new TagDto() { Name = x.Name, Id = x.Id }).ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while loading receipts from database",
                ex.Message);
        }
    }

    public async Task<Result<TagDto?>> GetByIdAsync(Guid id)
    {
        try
        {
            return await context.Tags.Select(x => new TagDto()
            {
                Name = x.Name,
                Id = x.Id
            }).FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while loading receipts from database",
                ex.Message);
        }
    }
    
    public async Task<Result<ICollection<TagDto>>> GetTagsByName(string name)
    {
        try
        {
            return await context.Tags
                .Where(x => x.Name.Contains(name))
                .Select(x => new TagDto()
                {
                    Name = x.Name,
                    Id = x.Id
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Error((int)HttpStatusCode.InternalServerError, "Error while loading receipts from database",
                ex.Message);
        }
    }
}