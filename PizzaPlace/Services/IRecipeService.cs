using PizzaPlace.Models;

namespace PizzaPlace.Services;

public interface IRecipeService
{
    Task<ComparableList<PizzaRecipeDto>> GetPizzaRecipes(PizzaOrder order);
    Task<PizzaRecipeDto> AddPizzaRecipe(PizzaRecipeDto recipe);
    Task<PizzaRecipeDto> UpdatePizzaRecipe(PizzaRecipeDto recipe);
}
