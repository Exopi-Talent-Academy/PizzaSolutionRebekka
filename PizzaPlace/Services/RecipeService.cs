using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
{
    /// <summary>
    /// Get the list of distinct recipes in an order
    /// </summary>
    /// <param name="order"></param>
    /// <returns>list of pizza recipes</returns>
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

    /// <summary>
    /// Adds a pizza recipe to the repository
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns>id of the new recipe</returns>
    public async Task<long> AddPizzaRecipe(PizzaRecipeDto recipe)
    {
        try
        {
            return await recipeRepository.AddRecipe(recipe);
        }
        catch (PizzaException)
        {
            throw;
        }
    }

    /// <summary>
    /// Updates an existing pizza recipe with new information
    /// </summary>
    /// <param name="updatedRecipe"></param>
    /// <returns>id of the updated recipe</returns>
    /// <exception cref="PizzaException"></exception>
    public async Task<long> UpdatePizzaRecipe(PizzaRecipeDto updatedRecipe)
    {
        long existingRecipeID;
        
        try
        {
            var existingRecipe = await recipeRepository.GetRecipe(updatedRecipe.RecipeType);
            existingRecipeID = existingRecipe.Id;
        }
        catch (PizzaException)
        {
            throw;
        }

        return await recipeRepository.UpdateRecipe(existingRecipeID, updatedRecipe);
    }
}
