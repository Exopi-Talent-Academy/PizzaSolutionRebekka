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

    [TestMethod]
    [DataRow(1, 2, 20)]
    [DataRow(5, 1, 6)]
    [DataRow(5, 2, 4)]
    public async Task HasInsufficentStock_ReturnsTrueWhenInsufficient(int orderAmount1, int orderAmount2, int orderAmount3)
    {
        // Arrange : Make the order and recipes
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount1), // need to do casting because DataRow doesn't take ushorts
            new PizzaAmount(PizzaRecipeType.OddPizza, (ushort)orderAmount2),
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount3)
        ]);

        StockDto recipeStock1 = new StockDto(StockType.UnicornDust, 1);
        StockDto recipeStock2 = new StockDto(StockType.Sulphur, 10);
        var rareRecipe = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [recipeStock1], 1);
        var oddRecipe = new PizzaRecipeDto(PizzaRecipeType.OddPizza, [recipeStock2], 100);
        ComparableList<PizzaRecipeDto> recipeList = [rareRecipe, oddRecipe];

        // Arrange : Make the mock recipe repository with recipes
        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.RarePizza))
            .ReturnsAsync(rareRecipe);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.OddPizza))
            .ReturnsAsync(oddRecipe);

        // Arrange : Make the mock stock repository with the stock
        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);

        StockDto mockStock1 = new StockDto(StockType.UnicornDust, 10);
        StockDto mockStock2 = new StockDto(StockType.Sulphur, 10);

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
    [DataRow(2, 1, 4)]
    [DataRow(5, 1, 5)]
    [DataRow(9, 1, 1)]
    public async Task HasInsufficentStock_ReturnsFalseWhenSufficient(int orderAmount1, int orderAmount2, int orderAmount3)
    {
        // Arrange : Make the order and recipes
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount1), // need to do casting because DataRow doesn't take ushorts
            new PizzaAmount(PizzaRecipeType.OddPizza, (ushort)orderAmount2),
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount3)
        ]);

        StockDto recipeStock1 = new StockDto(StockType.UnicornDust, 1);
        StockDto recipeStock2 = new StockDto(StockType.Sulphur, 10);
        var rareRecipe = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [recipeStock1], 1);
        var oddRecipe = new PizzaRecipeDto(PizzaRecipeType.OddPizza, [recipeStock2], 100);
        ComparableList<PizzaRecipeDto> recipeList = [rareRecipe, oddRecipe];

        // Arrange : Make the mock recipe repository with recipes
        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.RarePizza))
            .ReturnsAsync(rareRecipe);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.OddPizza))
            .ReturnsAsync(oddRecipe);

        // Arrange : Make the mock stock repository with the stock
        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);

        StockDto mockStock1 = new StockDto(StockType.UnicornDust, 10);
        StockDto mockStock2 = new StockDto(StockType.Sulphur, 10);

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
    [DataRow(1, 1, 5)]
    [DataRow(5, 1, 5)]
    [DataRow(4, 1, 3)]
    public async Task GetStock_GivesCorrectStock(int orderAmount1, int orderAmount2, int orderAmount3)
    {
        // Arrange : Make the order and recipes
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount1), // need to do casting because DataRow doesn't take ushorts
            new PizzaAmount(PizzaRecipeType.OddPizza, (ushort)orderAmount2),
            new PizzaAmount(PizzaRecipeType.RarePizza, (ushort)orderAmount3)
        ]);

        StockDto recipeStock1 = new StockDto(StockType.UnicornDust, 1);
        StockDto recipeStock2 = new StockDto(StockType.Sulphur, 10);
        var rareRecipe = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [recipeStock1], 1);
        var oddRecipe = new PizzaRecipeDto(PizzaRecipeType.OddPizza, [recipeStock2], 100);
        ComparableList<PizzaRecipeDto> recipeList = [rareRecipe, oddRecipe];

        // Arrange : Make the mock recipe repository with recipes
        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.RarePizza))
            .ReturnsAsync(rareRecipe);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.OddPizza))
            .ReturnsAsync(oddRecipe);

        // Arrange : Make the mock stock repository with the stock
        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);

        StockDto mockStock1 = new StockDto(StockType.UnicornDust, 10);
        StockDto mockStock2 = new StockDto(StockType.Sulphur, 10);

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
