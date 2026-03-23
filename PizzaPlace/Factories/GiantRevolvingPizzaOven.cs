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

    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) // cooking time last
    //{
    //    List<(PizzaRecipeDto, Guid)> orderList = recipeOrders.ToList(); // making it into a list that things can be removed from
    //    List<List<(PizzaRecipeDto, Guid)>> orderListByCookingTime = []; // cannot be a dictionary because it needs to allow cooking time repeats

    //    while (orderList.Count != 0)
    //    {
    //        var firstOrder = orderList.First();
    //        List<(PizzaRecipeDto, Guid)> subOrderList = [];
    //        int noOfSameCookingTimePizzas = 0;

    //        foreach (var order in orderList)
    //        {
    //            if (order.Item1.CookingTimeMinutes == firstOrder.Item1.CookingTimeMinutes)
    //            {
    //                subOrderList.Add(order);
    //                orderList.Remove(order);
    //                noOfSameCookingTimePizzas++;
    //            }

    //            if (noOfSameCookingTimePizzas == GiantRevolvingPizzaOvenCapacity)
    //            {
    //                break;
    //            }
    //        }

    //        orderListByCookingTime.Add(subOrderList);
    //    }

    //    // Only the last element in a sublist should have the proper cooking time, the rest should be zero
    //    foreach (var sublist in orderListByCookingTime)
    //    {
    //        foreach (var order in sublist)
    //        {
    //            if (order == sublist.Last())
    //            {
    //                _pizzaQueue.Enqueue((MakePizza(order.Item1), order.Item2));
    //            }
    //            else
    //            {
    //                var newRecipeCookingTime = order.Item1 with { CookingTimeMinutes = 0 };
    //                _pizzaQueue.Enqueue((MakePizza(newRecipeCookingTime), order.Item2));
    //            }
    //        }
    //    }
    //}

    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) // cooking time first
    //{
    //    List<(PizzaRecipeDto, Guid)> orderList = recipeOrders.ToList(); // making it into a list that things can be removed from

    //    List<(PizzaRecipeDto, Guid)> seenOrdersList = [];

    //    int breakFlag = orderList.Count;
    //    while (breakFlag != 0)
    //    {
    //        var lastOrder = orderList[breakFlag - 1];
    //        int noOfSameCookingTimePizzas = 0;

    //        foreach (var order in orderList)
    //        {
    //            if (seenOrdersList.Contains(order))
    //            {
    //                breakFlag--;
    //                continue;
    //            }

    //            if (order.Item1.CookingTimeMinutes == lastOrder.Item1.CookingTimeMinutes)
    //            {
    //                if (order.Item2 != lastOrder.Item2)
    //                {
    //                    var newRecipeCookingTime = order.Item1 with { CookingTimeMinutes = 0 };
    //                    _pizzaQueue.Enqueue((MakePizza(newRecipeCookingTime), order.Item2));
    //                }
    //                else
    //                {
    //                    _pizzaQueue.Enqueue((MakePizza(order.Item1), order.Item2));
    //                }

    //                seenOrdersList.Add(order);
    //                noOfSameCookingTimePizzas++;
    //            }

    //            breakFlag--;

    //            if (noOfSameCookingTimePizzas == GiantRevolvingPizzaOvenCapacity)
    //            {
    //                break;
    //            }
    //        }
    //    }
    //}

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) // cooking time first
    {
        List<(PizzaRecipeDto, Guid)> orderList = recipeOrders.ToList(); // making it into a list that things can be removed from

        List<(int, int)> seenCookingTimes = [];

        int breakFlag = 0;
        while (breakFlag != orderList.Count)
        {
            var firstOrder = orderList[breakFlag - 1];
            int noOfSameCookingTimePizzas = 0;

            for (int i = breakFlag; i > orderList.Count; i++)
            {
                if (order.Item1.CookingTimeMinutes == firstOrder.Item1.CookingTimeMinutes)
                {
                    if (seenCookingTimes.FindLast(x => x.Item1.CookingTime));

                    if (order.Item2 != firstOrder.Item2)
                    {
                        var newRecipeCookingTime = order.Item1 with { CookingTimeMinutes = 0 };
                        _pizzaQueue.Enqueue((MakePizza(newRecipeCookingTime), order.Item2));
                    }
                    else
                    {
                        _pizzaQueue.Enqueue((MakePizza(order.Item1), order.Item2));
                    }

                    seenCookingTimes.Add(order);
                    noOfSameCookingTimePizzas++;
                }

                breakFlag++;

                if (noOfSameCookingTimePizzas == GiantRevolvingPizzaOvenCapacity)
                {
                    break;
                }
            }
        }
    }

    private Func<Task<Pizza?>> MakePizza(PizzaRecipeDto recipe) => async () =>
    {
        await CookPizza(recipe.CookingTimeMinutes);

        return GetPizza(recipe.RecipeType);
    };

    //NOT IMPLEMENTED CORRECTLY
    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
    //{
    //    List<(PizzaRecipeDto, Guid)> orderList = recipeOrders.ToList(); // making it into a list that things can be removed from

    //    // Until the list is empty, run through it and create new sublists for each cooking time
    //    while (orderList.Any())
    //    {
    //        var firstOrder = orderList.First();
    //        List<(PizzaRecipeDto, Guid)> sublist = [];

    //        foreach (var order in orderList)
    //        {
    //            if (order.Item1.CookingTimeMinutes == firstOrder.Item1.CookingTimeMinutes)
    //            {
    //                sublist.Add(order);
    //                orderList.Remove(order);
    //            }
    //        }

    //        // Cook the pizzas
    //        MakePizzas(sublist);

    //        foreach (var (recipe, guid) in sublist)
    //        {
    //            _pizzaQueue.Enqueue((GetPizzas(recipe.RecipeType), guid));
    //        }
    //    }
    //}

    //private async void MakePizzas(List<(PizzaRecipeDto, Guid)> recipes)
    //{
    //    // They all have the same cooking time, so we only need to wait for one of them for each 120

    //    // If the list is longer than the oven capacity, then we have to take it in chunks
    //    while (recipes.Any() && (recipes.Count > GiantRevolvingPizzaOvenCapacity))
    //    {
    //        await CookPizza(recipes[0].Item1.CookingTimeMinutes);
    //        recipes.RemoveRange(0, GiantRevolvingPizzaOvenCapacity);
    //    }

    //    // The last time we wait for them to finish. Or the first, if there's 120 or less recipes to cook
    //    await CookPizza(recipes[0].Item1.CookingTimeMinutes);
    //}

    //private Func<Task<Pizza?>> GetPizzas(PizzaRecipeType recipeType) => async () =>
    //{
    //    return GetPizza(recipeType);
    //};
}
