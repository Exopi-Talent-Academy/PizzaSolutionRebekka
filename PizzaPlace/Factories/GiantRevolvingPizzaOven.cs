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

    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) // cooking time first
    //{
    //    List<(PizzaRecipeDto, Guid)> orderList = recipeOrders.ToList(); // making it into a list that things can be done with

    //    List<Guid> seenOrders = [];

    //    int breakFlag = 0;
    //    while (breakFlag < orderList.Count)
    //    {
    //        var firstOrder = orderList[breakFlag];
    //        int noOfTimesSeenBefore = 0;

    //        if (seenOrders.Contains(firstOrder.Item2))
    //        {
    //            breakFlag++;
    //            continue;
    //        }

    //        for (int i = breakFlag; i < orderList.Count; i++)
    //        {
    //            var order = orderList[i];
    //            PizzaRecipeDto orderRecipe = order.Item1;
    //            Guid orderGuid = order.Item2;

    //            if (seenOrders.Contains(orderGuid))
    //            {
    //                continue;
    //            }

    //            if (orderRecipe.CookingTimeMinutes == firstOrder.Item1.CookingTimeMinutes)
    //            {
    //                if (noOfTimesSeenBefore == GiantRevolvingPizzaOvenCapacity)
    //                {
    //                 //   If the cooking time has already been seen but is at full capacity
    //                   noOfTimesSeenBefore = 0;
    //                    _pizzaQueue.Enqueue((MakePizza(orderRecipe), orderGuid));
    //                }
    //                else if (orderGuid != firstOrder.Item2)
    //                {
    //                 //   If the cooking time has already been seen and is not at full capacity
    //                   var newRecipeCookingTime = orderRecipe with { CookingTimeMinutes = 0 };
    //                    _pizzaQueue.Enqueue((MakePizza(newRecipeCookingTime), orderGuid));
    //                }
    //                else
    //                {
    //                 //   If this is the first time a cooking time is seen
    //                   _pizzaQueue.Enqueue((MakePizza(orderRecipe), orderGuid));
    //                }

    //                noOfTimesSeenBefore++;
    //                seenOrders.Add(orderGuid);
    //            }
    //        }

    //        breakFlag++;
    //    }
    //}

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) // cooking time first
    {
        var orderList = recipeOrders.OrderBy(x => x.Recipe.CookingTimeMinutes).ToList();
        int noOfTimesSeenBefore = 0;
        int previousCookingTime = -1; // set to unallowed cooking time
        bool isFirst = true;

        foreach (var (orderRecipe, orderGuid) in orderList)
        {
            if (previousCookingTime == orderRecipe.CookingTimeMinutes)
            {
                if (noOfTimesSeenBefore == GiantRevolvingPizzaOvenCapacity)
                {
                    noOfTimesSeenBefore = 0;
                    _pizzaQueue.Enqueue((MakePizza(orderRecipe), orderGuid));
                }
                else
                {
                    var newRecipeCookingTime = orderRecipe with { CookingTimeMinutes = 0 };
                    _pizzaQueue.Enqueue((MakePizza(newRecipeCookingTime), orderGuid));
                }
            } 
            else
            {
                if (!isFirst)
                {
                    // INSERT EMPTY NUMBER OF ORDERS DEPENDING ON THE noOfTimesSeenBefore
                    for (int i = noOfTimesSeenBefore; i < GiantRevolvingPizzaOvenCapacity; i++)
                    {
                        PizzaRecipeDto emptyPizza = new PizzaRecipeDto(PizzaRecipeType.EmptyPizza, [], 0);
                        _pizzaQueue.Enqueue((MakePizza(emptyPizza), new Guid()));
                    }
                }

                isFirst = false;
                noOfTimesSeenBefore = 0;
                _pizzaQueue.Enqueue((MakePizza(orderRecipe), orderGuid));
            }

            noOfTimesSeenBefore++;
            previousCookingTime = orderRecipe.CookingTimeMinutes;
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
