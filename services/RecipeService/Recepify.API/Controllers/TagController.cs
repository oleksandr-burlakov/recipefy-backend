using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Models.Receipt;
using Recepify.BLL.Models.Tag;
using Recepify.BLL.Services;
using Recepify.Core.ResultPattern;

namespace Recepify.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TagController(TagService tagService) : ControllerBase
{

    [HttpGet]
    public async Task<IResultBase> GetAllAsync()
    {
        return await tagService.GetAllAsync();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IResultBase> GetTagById(Guid id)
    {
        return await tagService.GetByIdAsync(id);
    }
    
    [HttpGet("search/{name}")]
    public async Task<IResultBase> GetTagsByName(string name)
    {
        return await tagService.GetTagsByName(name);
    }
    
    [HttpPost]
    public async Task<IResultBase> AddTag([FromBody] AddTagDto tagRequest)
    {
        return await tagService.AddAsync(tagRequest);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IResultBase> DeleteTag(Guid id)
    {
        return await tagService.DeleteAsync(id);
    }

}
