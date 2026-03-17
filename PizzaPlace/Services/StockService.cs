using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

public class StockService(IStockRepository stockRepository) : IStockService
{
    public Task<bool> HasInsufficientStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        ComparableList<StockDto> stockNeededForOrder = new ComparableList<StockDto>();

        // NEED TO TAKE INTO ACCOUNT THAT THE ORDER DECIDES HOW MANY OF SOMETHING THERE IS
        // BOTH ORDER AND RECIPEDTOS HAVE A PIZZARECIPETYPE, PAIR 'EM UP

        // Add together the PizzaAmounts in order where the PizzaType is the same?? Would change what I do for quantity

        // Run through each StockType used in the order and add the ones who are the same together in a new stockdto
        foreach (PizzaRecipeDto recipe in recipeDtos) 
        {
            // Get the quantity of a specific pizza in an order
            int quantity = 0;

            foreach (var pizza in order.RequestedOrder)
            {
                if (pizza.PizzaType == recipe.RecipeType)
                {
                    quantity = pizza.Amount;
                    break;
                }
            }

            foreach (StockDto stock in recipe.Ingredients)
            {
                StockDto newStock = stock with { Amount = (stock.Amount * quantity) };

                if (stockNeededForOrder.Any(item => item.StockType == stock.StockType))
                {
                    // If this stocktype is already in the list, add to it
                    //CODE
                }
                else
                {
                    // If it's not already in the list, add it
                    stockNeededForOrder.Add(newStock);
                }
            }
        }

        // Then call GetStock from FakeStockRepository and compare with the above, if there's ever too many, return false

        // Call GetStock from FakeStockRepository for each thing in the order and check if there's enough stock

        return false;
    }

    public Task<ComparableList<StockDto>> GetStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        throw new NotImplementedException("Getting stock must be implemented.");

        // Call GetStock from FakeStockRepository
    }
}
