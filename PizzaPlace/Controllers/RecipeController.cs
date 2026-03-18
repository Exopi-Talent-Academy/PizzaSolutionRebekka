using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/recipes")]
public class RecipeController(IRecipeService recipeService) : ControllerBase
{
    /*[HttpGet]
    public IActionResult GetRecipes()
    {
        return Ok(recipeService.GetPizzaRecipes());
    }*/

    //[HttpPost]
}
