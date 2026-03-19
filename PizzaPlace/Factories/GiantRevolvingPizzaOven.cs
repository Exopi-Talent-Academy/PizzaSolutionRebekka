using PizzaPlace.Models;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

/// <summary>
/// Produces pizza on one giant revolving surface. Downside is, that all pizzas must have the same cooking time, when prepared at the same time.
/// </summary>
public class GiantRevolvingPizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
{
    private const int GiantRevolvingPizzaOvenCapacity = 120;

    protected override int Capacity => GiantRevolvingPizzaOvenCapacity;

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
    {
        // Turn the recipeOrders into a normal list that things can be removed from
        List<(PizzaRecipeDto, Guid)> list = recipeOrders.ToList();

        // Until the list is empty, run through it and create new MakePizzas for each cooking time
        while (list.Any())
        {
            var firstOrder = list.First();
            List<(PizzaRecipeDto, Guid)> sublist = [];

            foreach (var order in list)
            {
                if (order.Item1.CookingTimeMinutes == firstOrder.Item1.CookingTimeMinutes)
                {
                    sublist.Add(order);
                    list.Remove(order);
                }
            }

            // Add to queue
            _pizzaQueue.Enqueue((MakePizzas(sublist), new Guid()));
        }
    }

    private Func<Task<Pizza?>> MakePizzas(List<(PizzaRecipeDto, Guid)> recipes) => async () =>
    {
        // They all have the same cooking time, so we only need to wait for one of them for each 120

        // If the list is longer than the oven capacity, then we have to take it in chunks
        while (recipes.Any() && (recipes.Count >= GiantRevolvingPizzaOvenCapacity))
        {
            // We wait for the pizzas to cook and then remove them
            await CookPizza(recipes[0].Item1.CookingTimeMinutes);
            recipes.RemoveRange(0, GiantRevolvingPizzaOvenCapacity);
        }

        // The last time we wait for them to finish. Or the first, if there's 120 or less recipes to cook
        await CookPizza(recipes[0].Item1.CookingTimeMinutes);

        // Return each finished pizza?
        // WHAT IN THE WORLD DO I DO HERE SINCE THE RECIPES CAN BE OF MANY DIFFERENT TYPES
        return GetPizza(recipes[0].Item1.RecipeType);
    };
}
