using PizzaPlace.Models;
using PizzaPlace.Models.Types;
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
        List<(PizzaRecipeDto, Guid)> orderList = recipeOrders.ToList();

        // Until the list is empty, run through it and create new MakePizzas for each cooking time
        while (orderList.Any())
        {
            var firstOrder = orderList.First();
            List<(PizzaRecipeDto, Guid)> sublist = [];

            foreach (var order in orderList)
            {
                if (order.Item1.CookingTimeMinutes == firstOrder.Item1.CookingTimeMinutes)
                {
                    sublist.Add(order);
                    orderList.Remove(order);
                }
            }

            // Cook the pizzas
            MakePizzas(sublist);

            // Add to queue
            foreach (var (recipe, guid) in sublist)
            {
                _pizzaQueue.Enqueue((GetPizzas(recipe.RecipeType), guid));
            }
        }
    }

    private async void MakePizzas(List<(PizzaRecipeDto, Guid)> recipes)
    {
        // They all have the same cooking time, so we only need to wait for one of them for each 120

        // If the list is longer than the oven capacity, then we have to take it in chunks
        while (recipes.Any() && (recipes.Count > GiantRevolvingPizzaOvenCapacity))
        {
            // We wait for the pizzas to cook and then remove them
            await CookPizza(recipes[0].Item1.CookingTimeMinutes);
            recipes.RemoveRange(0, GiantRevolvingPizzaOvenCapacity);
        }

        // The last time we wait for them to finish. Or the first, if there's 120 or less recipes to cook
        await CookPizza(recipes[0].Item1.CookingTimeMinutes);
    }

    private Func<Task<Pizza?>> GetPizzas(PizzaRecipeType recipeType) => async () =>
    {
        return GetPizza(recipeType);
    };
}
