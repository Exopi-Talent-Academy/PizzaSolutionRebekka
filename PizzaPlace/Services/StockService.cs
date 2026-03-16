using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

public class StockService(IStockRepository stockRepository) : IStockService
{
    public Task<bool> HasInsufficientStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        throw new NotImplementedException("Sufficient stock must be checked.");

        // Call GetStock from FakeStockRepository for each thing in the order and check if there's enough stock
    }

    public Task<ComparableList<StockDto>> GetStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        throw new NotImplementedException("Getting stock must be implemented.");

        // Call GetStock from FakeStockRepository
    }
}
