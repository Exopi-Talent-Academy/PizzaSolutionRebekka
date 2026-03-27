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

    // Give 0 cooking time to all pizzas that aren't the first of their batch
    // Only works with 100 pizzas
    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
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

    // Give 0 cooking time to all pizzas that aren't the last of their batch
    // Only works with 100 pizzas
    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
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

    // Give 0 cooking time to all pizzas that aren't the first of their batch, now with a sorted list so it's way cleaner
    // Only works with 100 pizzas
    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
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

    // Batching and giving empty pizzas with the 0 cooking time (made by Copilot)
    // Keeps running without end
    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
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

    // Batching and giving empty pizzas with the correct cooking time
    // Keeps running without end
    //protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
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

    // My final attempt to try to wait for each batch so there was no need for empty pizzas
    // Fails all tests
    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
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
        await CookPizza(batch[0].Item1.CookingTimeMinutes); // this is basically the same as when I was only giving the first pizza the cooking time

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
