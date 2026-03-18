using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
{
    /// <summary>
    /// Returns a list of the distinct recipes in an order
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    // And by distinct is meant per PizzaRecipeType because they only have one recipe each
    public async Task<ComparableList<PizzaRecipeDto>> GetPizzaRecipes(PizzaOrder order)
    {
        var pizzaTypes = order.RequestedOrder
            .Select(x => x.PizzaType)
            .Distinct()
            .ToList();

        ComparableList<PizzaRecipeDto> recipes = [];
        foreach (var pizzaType in pizzaTypes)
        {
            recipes.Add(await recipeRepository.GetRecipe(pizzaType));
        }

        return recipes;
    }

    public async Task<PizzaRecipeDto> AddPizzaRecipe(PizzaRecipeDto recipe)
    {
        throw new NotImplementedException();

        // Try to recipeRepository.AddRecipe
    }

    public async Task<PizzaRecipeDto> UpdatePizzaRecipe(PizzaRecipeDto recipe)
    {
        throw new NotImplementedException();

        // Try to recipeRepository.GetRecipe
    }
}
