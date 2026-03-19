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
        // Dictionary that holds the cooking time of the last time a recipe was used
        Dictionary<PizzaRecipeType, int> previousCookingTime = new Dictionary<PizzaRecipeType, int>();

        // Go through each thing in the order, manipulate its cooking time depending on the last cooking time and add it to the _pizzaQueue
        foreach (var (recipe, orderGuid) in recipeOrders)
        {
            int newCookingTimeInMinutes;

            if (previousCookingTime.ContainsKey(recipe.RecipeType))
            {
                // If the PizzaRecipeType has been seen before, edit the cooking time dependent on the last one
                newCookingTimeInMinutes = previousCookingTime[recipe.RecipeType] - SubsequentPizzaTimeSavingsInMinutes;

                // Check if it has reached minimum or below
                if (newCookingTimeInMinutes <= MinimumCookingTimeMinutes)
                {
                    // If true, ensure they are the minimum
                    previousCookingTime[recipe.RecipeType] = MinimumCookingTimeMinutes;
                    newCookingTimeInMinutes = MinimumCookingTimeMinutes;
                }
                else
                {
                    // If the cooking time hasn't yet reached below the minimum, change the dictionary to reflect it
                    previousCookingTime[recipe.RecipeType] = newCookingTimeInMinutes;
                }
            }
            else 
            {
                // If this PizzaRecipeType hasn't been seen before
                // Add to the list and since it's the first time, it gets the setup minutes
                newCookingTimeInMinutes = recipe.CookingTimeMinutes + SetupTimeMinutes;
                previousCookingTime.Add(recipe.RecipeType, newCookingTimeInMinutes);
            }

            // Add to queue with the changed cooking time
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
