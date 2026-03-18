using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class StockServiceTests
{
    private static StockService GetService(Mock<IStockRepository> stockRepository) =>
        new(stockRepository.Object);

    private StockDto recipeStock1 = new StockDto(StockType.UnicornDust, 1);
    private StockDto recipeStock2 = new StockDto(StockType.Sulphur, 10);
    private StockDto mockStock1 = new StockDto(StockType.UnicornDust, 10);
    private StockDto mockStock2 = new StockDto(StockType.Sulphur, 10);

    [TestMethod]
    [DataRow(1, 2, 20)] // Checks that it works when pizzas of different types exceed the stock
    [DataRow(5, 1, 6)]  // Checks that it works when there are two pizzas of the same type that separately don't exceed the stock, but do in combination
    [DataRow(5, 2, 4)]  // Checks that it works when just one exceeds
    public async Task HasInsufficentStock_ReturnsTrueWhenInsufficient(int orderAmount1, int orderAmount2, int orderAmount3)
    {
        // Arrange : Make the order and recipes
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount1), // need to do casting because DataRow doesn't take ushorts
            new PizzaAmount(PizzaRecipeType.OddPizza, (ushort)orderAmount2),
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount3)
        ]);

        ComparableList<PizzaRecipeDto> recipeList = [new PizzaRecipeDto(PizzaRecipeType.RarePizza, [recipeStock1], 1), 
                                                     new PizzaRecipeDto(PizzaRecipeType.OddPizza, [recipeStock2], 100)];

        // Arrange : Make the mock stock repository
        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);

        stockRepository.Setup(x => x.GetStock(recipeStock1.StockType))
            .ReturnsAsync(mockStock1);
        stockRepository.Setup(x => x.GetStock(recipeStock2.StockType))
            .ReturnsAsync(mockStock2);

        var service = GetService(stockRepository);

        // Act
        var result = await service.HasInsufficientStock(order, recipeList);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow(2, 1, 4)] // Checks that it works when hitting the exact amount in the stock for one
    [DataRow(5, 1, 5)] // Checks that it works when hitting the exact amount in the stock for two
    [DataRow(1, 1, 1)] // Basic test case
    public async Task HasInsufficentStock_ReturnsFalseWhenSufficient(int orderAmount1, int orderAmount2, int orderAmount3)
    {
        // Arrange : Make the order and recipes
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount1), // need to do casting because DataRow doesn't take ushorts
            new PizzaAmount(PizzaRecipeType.OddPizza, (ushort)orderAmount2),
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount3)
        ]);

        ComparableList<PizzaRecipeDto> recipeList = [new PizzaRecipeDto(PizzaRecipeType.RarePizza, [recipeStock1], 1), 
                                                     new PizzaRecipeDto(PizzaRecipeType.OddPizza, [recipeStock2], 100)];

        // Arrange : Make the mock stock repository
        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);

        stockRepository.Setup(x => x.GetStock(recipeStock1.StockType))
            .ReturnsAsync(mockStock1);
        stockRepository.Setup(x => x.GetStock(recipeStock2.StockType))
            .ReturnsAsync(mockStock2);

        var service = GetService(stockRepository);

        // Act
        var result = await service.HasInsufficientStock(order, recipeList);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow(1, 1, 5)] // Basic test case where it doesn't go above the limit
    [DataRow(5, 1, 5)] // Basic test case where it doesn't go above the limit
    [DataRow(4, 1, 3)] // Basic test case where it doesn't go above the limit
    public async Task GetStock_GivesCorrectStock(int orderAmount1, int orderAmount2, int orderAmount3)
    {
        // Arrange : Make the order and recipes
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount1), // need to do casting because DataRow doesn't take ushorts
            new PizzaAmount(PizzaRecipeType.OddPizza, (ushort)orderAmount2),
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount3)
        ]);

        ComparableList<PizzaRecipeDto> recipeList = [new PizzaRecipeDto(PizzaRecipeType.RarePizza, [recipeStock1], 1), 
                                                     new PizzaRecipeDto(PizzaRecipeType.OddPizza, [recipeStock2], 100)];

        // Arrange : Make the mock stock repository
        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);

        stockRepository.Setup(x => x.GetStock(recipeStock1.StockType))
            .ReturnsAsync(mockStock1);
        stockRepository.Setup(x => x.GetStock(recipeStock2.StockType))
            .ReturnsAsync(mockStock2);

        var service = GetService(stockRepository);

        // Act
        var actual = await service.GetStock(order, recipeList);

        // Assert
        Assert.AreEqual(2, actual.Count); // the number of stock objects are the same
        Assert.AreEqual((order.RequestedOrder[0].Amount + order.RequestedOrder[2].Amount) * recipeStock1.Amount, 
                         actual.FirstOrDefault(item => item.StockType == recipeStock1.StockType)!.Amount);
        Assert.AreEqual(order.RequestedOrder[1].Amount * recipeStock2.Amount, 
                        actual.FirstOrDefault(item => item.StockType == recipeStock2.StockType)!.Amount);
    }
}
