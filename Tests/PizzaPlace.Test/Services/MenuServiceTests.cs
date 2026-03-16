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
    [DataRow(2026, 3, 16, 12, 12, 12)]
    [DataRow(2026, 3, 16, 11, 00, 00)]
    [DataRow(2026, 3, 16, 13, 59, 59)]
    public void GetMenu_GivesLunchMenuAtLunchtime(int year, int month, int day, int hour, int minute, int second)
    {
        // Arrange
        MenuService menuService = new MenuService();

        DateTimeOffset lunchTime = new DateTimeOffset(new DateTime(year, month, day, hour, minute, second));

        // Act
        Menu menu = menuService.GetMenu(lunchTime);

        // Assert
        Assert.AreEqual("Lunch Menu", menu.Title);
        Assert.AreEqual(12, menu.Items.Count);
        Assert.IsTrue(menu.Items.Any(item => item.Description == "Boo Pizza"));
        Assert.IsTrue(menu.Items.Any(item => item.Description == "Baby Mario Pizza"));
    }

    [TestMethod]
    [DataRow(2026, 3, 16, 14, 12, 12)]
    [DataRow(2026, 3, 16, 10, 59, 59)]
    [DataRow(2026, 3, 16, 23, 12, 12)]
    public void GetMenu_GivesStandardMenuOutsideLunchtime(int year, int month, int day, int hour, int minute, int second)
    {
        // Arrange
        MenuService menuService = new MenuService();

        DateTimeOffset notLunchTime = new DateTimeOffset(new DateTime(year, month, day, hour, minute, second));

        // Act
        Menu menu = menuService.GetMenu(notLunchTime);

        // Assert
        Assert.AreEqual("Standard Menu", menu.Title);
        Assert.AreEqual(12, menu.Items.Count);
        Assert.IsTrue(menu.Items.Any(item => item.Description == "Shy Guy Pizza"));
        Assert.IsTrue(menu.Items.Any(item => item.Description == "Bowser Jr Pizza"));
    }
}
