using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/recipes")]
public class RecipeController(IRecipeService recipeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddRecipe([FromBody] PizzaRecipeDto recipeDTO)
    {
        return Ok(recipeService.AddPizzaRecipe(recipeDTO));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecipe([FromBody] PizzaRecipeDto recipeDTO)
    {
        return Ok(recipeService.UpdatePizzaRecipe(recipeDTO));
    }
}
