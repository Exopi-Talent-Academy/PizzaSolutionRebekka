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
            var result = recipeService.AddPizzaRecipe(recipeDTO).Result;
            return Ok($"New recipe added with id {result}.");
        }
        catch (PizzaException)
        {
            return BadRequest("Recipe already exists. If you wish to update it, do that instead.");
        }
    }

    [HttpPut]
    public IActionResult UpdateRecipe([FromBody] PizzaRecipeDto recipeDTO)
    {
        try
        {
            var result = recipeService.UpdatePizzaRecipe(recipeDTO).Result;
            return Ok($"Recipe with id {result} successfully updated.");
        }
        catch (PizzaException)
        {
            return BadRequest("Recipe did not already exist. If you wish to add a new one, do that instead.");
        }
    }
}
