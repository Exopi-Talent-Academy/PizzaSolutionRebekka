using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

/// <summary>
/// Producing one line of pizza. 
/// Taking 7 minutes to setup - and then 5 minutes less for every subsequent pizza of the same recipe type to a minimum of 4 minutes.
/// </summary>
public class AssemblyLinePizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
{
    private const int AssemblyLineCapacity = 1;
    public const int SetupTimeMinutes = 7;
    public const int SubsequentPizzaTimeSavingsInMinutes = 5;
    public const int MinimumCookingTimeMinutes = 4;

    protected override int Capacity => AssemblyLineCapacity;

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
    {
        Dictionary<PizzaRecipeType, int> previousCookingTime = new Dictionary<PizzaRecipeType, int>();

        // Go through each thing in the order, manipulate its cooking time depending on the last cooking time and add it to the _pizzaQueue
        foreach (var (recipe, orderGuid) in recipeOrders)
        {
            int newCookingTimeInMinutes;

            if (previousCookingTime.ContainsKey(recipe.RecipeType))
            {
                newCookingTimeInMinutes = previousCookingTime[recipe.RecipeType] - SubsequentPizzaTimeSavingsInMinutes;

                if (newCookingTimeInMinutes <= MinimumCookingTimeMinutes)
                {
                    // If true, ensure they are the minimum
                    previousCookingTime[recipe.RecipeType] = MinimumCookingTimeMinutes;
                    newCookingTimeInMinutes = MinimumCookingTimeMinutes;
                }
                else
                {
                    previousCookingTime[recipe.RecipeType] = newCookingTimeInMinutes;
                }
            }
            else 
            {
                // If this PizzaRecipeType hasn't been seen before
                newCookingTimeInMinutes = recipe.CookingTimeMinutes + SetupTimeMinutes;
                previousCookingTime.Add(recipe.RecipeType, newCookingTimeInMinutes);
            }

            PizzaRecipeDto changedRecipe = recipe with { CookingTimeMinutes = newCookingTimeInMinutes };
            _pizzaQueue.Enqueue((MakePizza(changedRecipe), orderGuid));
        }
    }

    private Func<Task<Pizza?>> MakePizza(PizzaRecipeDto recipe) => async () =>
    {
        await CookPizza(recipe.CookingTimeMinutes);

        return GetPizza(recipe.RecipeType);
    };
}
