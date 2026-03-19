using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/recipes")]
public class RecipeController(IRecipeService recipeService) : ControllerBase
{
    [HttpPost]
    public IActionResult AddRecipe([FromBody] PizzaRecipeDto recipeDTO)
    {
        try
        {
            var result = recipeService.AddPizzaRecipe(recipeDTO);
            return Ok("New recipe added with id " + result);
        }
        catch (PizzaException)
        {
            return BadRequest("Recipe already exists. If you wish to update it, do that instead");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecipe([FromBody] PizzaRecipeDto recipeDTO)
    {
        return Ok(recipeService.UpdatePizzaRecipe(recipeDTO));
    }
}
