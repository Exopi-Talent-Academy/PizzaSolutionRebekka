using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class RecipeControllerTests
{
    private static RecipeController GetController(Mock<IRecipeService> recipeService) =>
        new(recipeService.Object);

    [TestMethod]
    public async Task AddRecipe()
    {
        // Arrange
        var recipe = new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [new StockDto(StockType.Tomatoes, 1)], 15);

        var recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
        recipeService.Setup(x => x.AddPizzaRecipe(recipe))
            .ReturnsAsync(1);

        var controller = GetController(recipeService);

        // Act
        var actual = controller.AddRecipe(recipe);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        recipeService.VerifyAll();
    }

    [TestMethod]
    public async Task UpdateRecipe()
    {
        // Arrange
        var recipe = new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [new StockDto(StockType.Tomatoes, 1)], 15);

        var recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
        recipeService.Setup(x => x.UpdatePizzaRecipe(recipe))
            .ReturnsAsync(1);

        var controller = GetController(recipeService);

        // Act
        var actual = controller.UpdateRecipe(recipe);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        recipeService.VerifyAll();
    }
}
