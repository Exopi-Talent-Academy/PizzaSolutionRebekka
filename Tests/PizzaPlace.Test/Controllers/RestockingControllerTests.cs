using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Repositories;
using PizzaPlace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class RestockingControllerTests
{
    private static RestockingController GetController(Mock<IStockRepository> stockRepository) =>
        new(stockRepository.Object);

    [TestMethod]
    public async Task Restock()
    {
        // Arrange
        ComparableList<StockDto> stockList = new ComparableList<StockDto>();
        StockDto newStock = new StockDto(Models.Types.StockType.Chocolate, 5);
        stockList.Add(newStock);

        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);
        stockRepository.Setup(x => x.AddToStock(newStock)).ReturnsAsync(newStock);

        var controller = GetController(stockRepository);

        // Act
        var actual = await controller.Restock(stockList);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        stockRepository.VerifyAll();
    }
}
