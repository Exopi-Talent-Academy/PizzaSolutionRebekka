using PizzaPlace.Repositories;
using PizzaPlace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PizzaPlace.Test.Services;

[TestClass]
public class MenuServiceTests
{
    [TestMethod]
    public void GetMenu_GivesLunchMenuAtLunchtime()
    {
        // Arrange
        MenuService menuService = new MenuService();

        DateTimeOffset lunchTime = new DateTimeOffset(new DateTime(2026, 3, 16, 12, 12, 12));

        // Act
        Menu menu = menuService.GetMenu(lunchTime);

        // Assert
        Assert.AreEqual("Lunch Menu", menu.Title);
        Assert.AreEqual(12, menu.Items.Count);
        Assert.IsTrue(menu.Items.Any(item => item.Description == "Boo Pizza"));
        Assert.IsTrue(menu.Items.Any(item => item.Description == "Baby Mario Pizza"));
    }

    [TestMethod]
    public void GetMenu_GivesStandardMenuOutsideLunchtime()
    {
        // Arrange
        MenuService menuService = new MenuService();

        DateTimeOffset pastLunchTime = new DateTimeOffset(new DateTime(2026, 3, 16, 14, 12, 12));

        // Act
        Menu menu = menuService.GetMenu(pastLunchTime);

        // Assert
        Assert.AreEqual("Standard Menu", menu.Title);
        Assert.AreEqual(12, menu.Items.Count);
        Assert.IsTrue(menu.Items.Any(item => item.Description == "Shy Guy Pizza"));
        Assert.IsTrue(menu.Items.Any(item => item.Description == "Bowser Jr Pizza"));
    }
}
