using PizzaPlace.Models.Types;

namespace PizzaPlace.Models;

/// <summary>
/// Holds information about a Pizza Recipe
/// </summary>
/// <param name="RecipeType"></param>
/// <param name="Ingredients"></param>
/// <param name="CookingTimeMinutes"></param>
/// <param name="Id"></param>
public record PizzaRecipeDto(PizzaRecipeType RecipeType, ComparableList<StockDto> Ingredients, int CookingTimeMinutes, long Id = 0) : Dto(Id);
