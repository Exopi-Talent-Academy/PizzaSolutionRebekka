using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

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

        // Go through the list of the stock needed for an order
        foreach (var neededStock in stockNeededForOrder)
        {
            StockDto currentStock = await stockRepository.GetStock(neededStock.StockType);

            // If the amount needed ever exceeds the amount in stock, return true because the stock is insufficient
            if (currentStock.Amount < neededStock.Amount)
            {
                return true;
            }
        }

        // The amount needed has never exceeded what's in stock, so return false
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
        ComparableList<StockDto> stockNeededForOrder = new ComparableList<StockDto>();
        Dictionary<PizzaRecipeType, int> recipeTypeAmountsInOrder = GetRecipeTypeAmountsInOrder(order.RequestedOrder);

        // Go through each recipe and add the needed stock to the list
        foreach (PizzaRecipeDto recipe in recipeDtos)
        {
            // Get the quantity of a specific pizzatype in an order
            int quantity = recipeTypeAmountsInOrder[recipe.RecipeType];

            // Go through the ingredients in the recipe and add them to the needed stock
            foreach (StockDto stock in recipe.Ingredients)
            {
                // The number of the same pizza ordered decides how much of an ingredient is needed
                StockDto newStock = stock with { Amount = (stock.Amount * quantity) };

                // Check if the ingredient's stocktype is already in the list
                if (stockNeededForOrder.Any(item => item.StockType == stock.StockType))
                {
                    // If this stocktype is already in the list, add to it
                    int index = stockNeededForOrder.IndexOf(stockNeededForOrder.FirstOrDefault(item => item.StockType == stock.StockType)!);
                    int newAmount = stockNeededForOrder[index].Amount + newStock.Amount;
                    stockNeededForOrder[index] = newStock with { Amount = newAmount };
                }
                else
                {
                    // If it's not already in the list, add it
                    stockNeededForOrder.Add(newStock);
                }
            }
        }

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
                // If any of them are of the same type, their amounts get added together
                recipeTypeAmountsInOrder[pizza.PizzaType] += pizza.Amount;
            }
            else
            {
                // If it's not in the list already, add it
                recipeTypeAmountsInOrder.Add(pizza.PizzaType, pizza.Amount);
            }
        }

        return recipeTypeAmountsInOrder;
    }
}
