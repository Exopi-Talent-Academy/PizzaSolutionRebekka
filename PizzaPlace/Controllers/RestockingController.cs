using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Controllers;

[Route("api/restocking")]
public class RestockingController(IStockRepository stockRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Restock([FromBody] ComparableList<StockDto> stock)
    {
        List<Task<StockDto>> tasks = new List<Task<StockDto>>();

        foreach (StockDto item in stock)
        {
            tasks.Add(stockRepository.AddToStock(item));
        }

        return Ok(new
        {
            // When all the items have been restocked, return ok
            stocks = await Task.WhenAll(tasks),
        });
    }
}
