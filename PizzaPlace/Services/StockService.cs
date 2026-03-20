using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Extensions;

namespace PizzaPlace.Services;

public class StockService(IStockRepository stockRepository) : IStockService
{
    /// <summary>
    /// Checks whether there are enough ingredients for an order
    /// </summary>
    /// <param name="order"></param>
    /// <param name="recipeDtos"></param>
    /// <returns>true, if the stock is insufficient; otherwise, false</returns>
    public async Task<bool> HasInsufficientStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        ComparableList<StockDto> stockNeededForOrder = await GetStock(order, recipeDtos);
        Dictionary<PizzaRecipeType, int> recipeTypeAmountsInOrder = GetRecipeTypeAmountsInOrder(order.RequestedOrder);

        foreach (var neededStock in stockNeededForOrder)
        {
            StockDto currentStock = await stockRepository.GetStock(neededStock.StockType);

            if (currentStock.Amount < neededStock.Amount)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the stock needed for an order
    /// </summary>
    /// <param name="order"></param>
    /// <param name="recipeDtos"></param>
    /// <returns>a ComparableList<StockDto> with the stock</returns>
    public async Task<ComparableList<StockDto>> GetStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        Dictionary<PizzaRecipeType, int> recipeTypeAmountsInOrder = GetRecipeTypeAmountsInOrder(order.RequestedOrder);

        IEnumerable<PizzaPrepareOrder> ordersToGetStockFrom = recipeDtos.Select(recipe => new PizzaPrepareOrder(recipe, recipeTypeAmountsInOrder[recipe.RecipeType]));

        ComparableList<StockDto> stockNeededForOrder = PizzaHelperExtensions.GetRequiredStock(ordersToGetStockFrom).ToComparableList();

        return stockNeededForOrder;
    }

    /// <summary>
    /// Get the amounts of types of PizzaRecipeType in dictionary from an order
    /// </summary>
    /// <param name="requestedOrder"></param>
    /// <returns>Dictionary with the PizzaRecipeType as the keys and their amounts as the value</returns>
    private Dictionary<PizzaRecipeType, int> GetRecipeTypeAmountsInOrder(ComparableList<PizzaAmount> requestedOrder)
    {
        Dictionary<PizzaRecipeType, int> recipeTypeAmountsInOrder = new Dictionary<PizzaRecipeType, int>();

        // Go through the ordered pizzas and add them to the dictionary with their amounts
        foreach (var pizza in requestedOrder)
        {
            if (recipeTypeAmountsInOrder.ContainsKey(pizza.PizzaType))
            {
                recipeTypeAmountsInOrder[pizza.PizzaType] += pizza.Amount;
            }
            else
            {
                recipeTypeAmountsInOrder.Add(pizza.PizzaType, pizza.Amount);
            }
        }

        return recipeTypeAmountsInOrder;
    }
}
