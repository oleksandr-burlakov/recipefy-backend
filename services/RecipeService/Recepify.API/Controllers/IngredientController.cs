using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recepify.BLL.Models.Ingredients;
using Recepify.BLL.Services;
using Recepify.Core.ResultPattern;

namespace Recepify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController(IngredientService ingredientService) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IResultBase> GetIngredientById(Guid id)
        {
            return await ingredientService.GetByIdAsync(id);
        }
        [HttpGet]
        public async Task<IResultBase> GetIngredientsByRecipeId([FromQuery] Guid recipeId)
        {
            return await ingredientService.GetByRecipeIdAsync(recipeId);
        }
        
        [HttpDelete]
        public async Task<IResultBase> DeleteIngredient(Guid id)
        {
            return await ingredientService.DeleteAsync(id);
        }
        
    }
}
