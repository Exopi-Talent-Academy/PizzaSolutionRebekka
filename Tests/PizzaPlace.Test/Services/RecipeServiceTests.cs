using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class RecipeServiceTests
{
    private static RecipeService GetService(Mock<IRecipeRepository> recipeRepository) =>
        new(recipeRepository.Object);

    [TestMethod]
    public async Task GetPizzaRecipes()
    {
        // Arrange
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, 1),
            new PizzaAmount(PizzaRecipeType.OddPizza, 2),
            new PizzaAmount(PizzaRecipeType.RarePizza, 20),
        ]);
        var rareRecipe = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 1);
        var oddRecipe = new PizzaRecipeDto(PizzaRecipeType.OddPizza, [new StockDto(StockType.Sulphur, 10)], 100);
        ComparableList<PizzaRecipeDto> expected = [rareRecipe, oddRecipe];

        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.RarePizza))
            .ReturnsAsync(rareRecipe);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.OddPizza))
            .ReturnsAsync(oddRecipe);

        var service = GetService(recipeRepository);

        // Act
        var actual = await service.GetPizzaRecipes(order);

        // Assert
        Assert.AreEqual(expected, actual);
        //recipeRepository.VerifyAll(); // this line was in the guide but was missing here
    }

    [TestMethod]
    public async Task AddRecipe_AddingOneRecipe()
    {
        // Arrange
        var recipe = new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [new StockDto(StockType.Tomatoes, 1)], 15);

        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.AddRecipe(recipe))
            .ReturnsAsync(1); // 1 is the first id a recipe can be given

        var service = GetService(recipeRepository);

        // Act
        var result = await service.AddPizzaRecipe(recipe);

        // Assert
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public async Task AddRecipe_AddingTwoRecipes()
    {
        // Arrange
        var recipe1 = new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [new StockDto(StockType.Tomatoes, 1)], 15);
        var recipe2 = new PizzaRecipeDto(PizzaRecipeType.OddPizza, [new StockDto(StockType.Bacon, 5)], 20);

        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.AddRecipe(recipe1))
            .ReturnsAsync(1); // 1 is the first id a recipe can be given
        recipeRepository.Setup(x => x.AddRecipe(recipe2))
            .ReturnsAsync(2);

        var service = GetService(recipeRepository);

        // Act
        var result1 = await service.AddPizzaRecipe(recipe1);
        var result2 = await service.AddPizzaRecipe(recipe2);

        // Assert
        Assert.AreEqual(1, result1);
        Assert.AreEqual(2, result2);
    }

    [TestMethod]
    public async Task UpdateRecipe_UpdatesRecipe()
    {
        // Arrange
        var oldRecipe = new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [new StockDto(StockType.Bacon, 1)], 15, 1);
        var updatedRecipe = new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [new StockDto(StockType.Tomatoes, 1)], 15);

        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.GetRecipe(updatedRecipe.RecipeType))
            .ReturnsAsync(oldRecipe);
        recipeRepository.Setup(x => x.UpdateRecipe(oldRecipe.Id, updatedRecipe))
            .ReturnsAsync(1); // 1 is the first id a recipe can be given

        var service = GetService(recipeRepository);

        // Act
        var result = await service.UpdatePizzaRecipe(updatedRecipe);

        // Assert
        Assert.AreEqual(oldRecipe.Id, result);
    }
}
