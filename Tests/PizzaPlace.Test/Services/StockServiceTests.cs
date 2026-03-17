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
    public async Task HasInsufficentStock_ReturnsTrueWhenInsufficient()
    {
        // Arrange : Make the order and recipes
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, 1),
            new PizzaAmount(PizzaRecipeType.OddPizza, 2),
            new PizzaAmount(PizzaRecipeType.RarePizza, 20),
        ]);

        StockDto stock1 = new StockDto(StockType.UnicornDust, 1);
        StockDto stock2 = new StockDto(StockType.Sulphur, 10);
        var rareRecipe = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [stock1], 1);
        var oddRecipe = new PizzaRecipeDto(PizzaRecipeType.OddPizza, [stock2], 100);
        ComparableList<PizzaRecipeDto> recipeList = [rareRecipe, oddRecipe];

        // Arrange : Make the repository with its stock
        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);
        stockRepository.Setup(x => x.AddToStock(stock1)).ReturnsAsync(stock1);
        stockRepository.Setup(x => x.AddToStock(stock2)).ReturnsAsync(stock2);

        var service = GetService(stockRepository);

        // Act
        var actual = await service.HasInsufficientStock(order, recipeList);

        // Assert
        Assert.IsFalse(actual);
    }
}
