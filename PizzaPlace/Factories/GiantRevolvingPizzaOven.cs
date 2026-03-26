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

    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) // cooking time last //fails all tests
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

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) // cooking time first //only works with 100 pizzas
    {
        List<(PizzaRecipeDto, Guid)> orderList = recipeOrders.ToList(); // making it into a list that things can be removed from

        List<(PizzaRecipeDto, Guid)> seenOrdersList = [];

        int breakFlag = orderList.Count;
        while (breakFlag != 0)
        {
            var lastOrder = orderList[breakFlag - 1];
            int noOfSameCookingTimePizzas = 0;

            foreach (var order in orderList)
            {
                if (seenOrdersList.Contains(order))
                {
                    breakFlag--;
                    continue;
                }

                if (order.Item1.CookingTimeMinutes == lastOrder.Item1.CookingTimeMinutes)
                {
                    if (order.Item2 != lastOrder.Item2)
                    {
                        var newRecipeCookingTime = order.Item1 with { CookingTimeMinutes = 0 };
                        _pizzaQueue.Enqueue((MakePizza(newRecipeCookingTime), order.Item2));
                    }
                    else
                    {
                        _pizzaQueue.Enqueue((MakePizza(order.Item1), order.Item2));
                    }

                    seenOrdersList.Add(order);
                    noOfSameCookingTimePizzas++;
                }

                breakFlag--;

                if (noOfSameCookingTimePizzas == GiantRevolvingPizzaOvenCapacity)
                {
                    break;
                }
            }
        }
    }

    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) //only works with 100 pizzas
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
    //                    //   If the cooking time has already been seen but is at full capacity
    //                    noOfTimesSeenBefore = 0;
    //                    _pizzaQueue.Enqueue((MakePizza(orderRecipe), orderGuid));
    //                }
    //                else if (orderGuid != firstOrder.Item2)
    //                {
    //                    //   If the cooking time has already been seen and is not at full capacity
    //                    var newRecipeCookingTime = orderRecipe with { CookingTimeMinutes = 0 };
    //                    _pizzaQueue.Enqueue((MakePizza(newRecipeCookingTime), orderGuid));
    //                }
    //                else
    //                {
    //                    //   If this is the first time a cooking time is seen
    //                    _pizzaQueue.Enqueue((MakePizza(orderRecipe), orderGuid));
    //                }

    //                noOfTimesSeenBefore++;
    //                seenOrders.Add(orderGuid);
    //            }
    //        }

    //        breakFlag++;
    //    }
    //}

    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) //only works with 100 pizzas
    //{
    //    var orderList = recipeOrders.OrderBy(x => x.Recipe.CookingTimeMinutes).ToList();
    //    int noOfTimesSeenBefore = 0;
    //    int previousCookingTime = -1; // set to unallowed cooking time

    //    foreach (var (orderRecipe, orderGuid) in orderList)
    //    {
    //        if (previousCookingTime == orderRecipe.CookingTimeMinutes && noOfTimesSeenBefore != GiantRevolvingPizzaOvenCapacity)
    //        {
    //            var newRecipeCookingTime = orderRecipe with { CookingTimeMinutes = 0 };
    //            _pizzaQueue.Enqueue((MakePizza(newRecipeCookingTime), orderGuid));
    //        }
    //        else
    //        {
    //            noOfTimesSeenBefore = 0;
    //            _pizzaQueue.Enqueue((MakePizza(orderRecipe), orderGuid));
    //        }

    //        noOfTimesSeenBefore++;
    //        previousCookingTime = orderRecipe.CookingTimeMinutes;
    //    }
    //}

    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) //keeps spinning
    //{
    //    var groups = recipeOrders.GroupBy(p => p.Recipe.CookingTimeMinutes).Select(g => g.ToList()).ToList();

    //    foreach (var group in groups)
    //    {
    //        var batches = group.Chunk(GiantRevolvingPizzaOvenCapacity);

    //        foreach (var batch in batches)
    //        {
    //            var list = batch.ToList();

    //            while (list.Count < GiantRevolvingPizzaOvenCapacity)
    //            {
    //                list.Add((new PizzaRecipeDto(PizzaRecipeType.EmptyPizza, [], 0), Guid.NewGuid()));
    //            }

    //            for (int i = 0; i < list.Count; i++)
    //            {
    //                var (recipe, guid) = list[i];
    //                var finalRecipe =
    //                    (i == 0) ? recipe :
    //                               recipe with { CookingTimeMinutes = 0 };

    //                _pizzaQueue.Enqueue((MakePizza(finalRecipe), guid));
    //            }
    //        }
    //    }
    //}

    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) //keeps spinning
    //{
    //    var groups = recipeOrders.GroupBy(p => p.Recipe.CookingTimeMinutes).Select(g => g.ToList()).ToList();

    //    foreach (var group in groups)
    //    {
    //        var batches = group.Chunk(GiantRevolvingPizzaOvenCapacity);

    //        foreach (var batch in batches)
    //        {
    //            var list = batch.ToList();

    //            while (list.Count < GiantRevolvingPizzaOvenCapacity)
    //            {
    //                list.Add((new PizzaRecipeDto(PizzaRecipeType.EmptyPizza, [], list[0].Recipe.CookingTimeMinutes), Guid.NewGuid()));
    //            }

    //            foreach (var (recipe, guid) in list)
    //            {
    //                _pizzaQueue.Enqueue((MakePizza(recipe), guid));
    //            }
    //        }
    //    }
    //}

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders) //fails all tests
    {
        var groups = recipeOrders.GroupBy(p => p.Recipe.CookingTimeMinutes).Select(g => g.ToList()).ToList();

        foreach (var group in groups)
        {
            var batches = group.Chunk(GiantRevolvingPizzaOvenCapacity);

            foreach (var batch in batches)
            {
                var list = batch.ToList();

                MakePizzas(list);

                foreach (var (recipe, guid) in list)
                {
                    _pizzaQueue.Enqueue((GetPizzaInPizzas(recipe), guid));
                }
            }
        }
    }

    private Func<Task<List<(PizzaRecipeDto, Guid)>>> MakePizzas(List<(PizzaRecipeDto, Guid)> batch) => async () =>
    {
        await CookPizza(batch[0].Item1.CookingTimeMinutes);

        return batch;
    };

    private async Task CookPizzas(List<(PizzaRecipeDto, Guid)> batch)
    {
        foreach (var (recipe, guid) in batch)
        {
            await CookPizza(recipe.CookingTimeMinutes);
        }
    }

    private Func<Task<Pizza?>> GetPizzaInPizzas(PizzaRecipeDto recipe) => async () =>
    {
        return GetPizza(recipe.RecipeType);
    };

    private Func<Task<Pizza?>> MakePizza(PizzaRecipeDto recipe) => async () =>
    {
        await CookPizza(recipe.CookingTimeMinutes);

        return GetPizza(recipe.RecipeType);
    };
}
