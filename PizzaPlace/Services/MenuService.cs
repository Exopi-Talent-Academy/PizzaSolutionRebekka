namespace PizzaPlace.Services;

public class MenuService : IMenuService
{
    /// <summary>
    /// Gets the menu depending on the time of day
    /// </summary>
    /// <param name="menuDate"></param>
    /// <returns></returns>
    public Menu GetMenu(DateTimeOffset menuDate)
    {
        int currentHour = menuDate.Hour;

        // Give the menu according to what time it is
        if (currentHour >= 11 && currentHour < 14)
        {
            // Return the lunch menu
            return new Menu("Lunch Menu", GetMenuItems(true));
        }
        else
        {
            // Return the standard menu
            return new Menu("Standard Menu", GetMenuItems(false));
        }
    }

    // Creating a hard-coded list of menu items
    private ComparableList<MenuItem> GetMenuItems(bool isLunchTime)
    {
        ComparableList<MenuItem> list = new ComparableList<MenuItem>();

        // Make the items that are always for sale and add them to the list
        list.Add(new MenuItem("Pepperoni Pizza", Models.Types.PizzaRecipeType.StandardPizza, 10.99));
        list.Add(new MenuItem("Mario Pizza", Models.Types.PizzaRecipeType.StandardPizza, 9.99));
        list.Add(new MenuItem("Luigi Pizza", Models.Types.PizzaRecipeType.StandardPizza, 9.99));
        list.Add(new MenuItem("Wario Pizza", Models.Types.PizzaRecipeType.OddPizza, 8.99));
        list.Add(new MenuItem("Waluigi Pizza", Models.Types.PizzaRecipeType.OddPizza, 10.99));
        list.Add(new MenuItem("Peach Pizza", Models.Types.PizzaRecipeType.ExtremelyTastyPizza, 12.99));
        list.Add(new MenuItem("Daisy Pizza", Models.Types.PizzaRecipeType.ExtremelyTastyPizza, 12.99));
        list.Add(new MenuItem("Yoshi Pizza", Models.Types.PizzaRecipeType.ExtremelyTastyPizza, 12.99));
        list.Add(new MenuItem("Mushroom Pizza", Models.Types.PizzaRecipeType.StandardPizza, 10.99));
        list.Add(new MenuItem("Bowser Pizza", Models.Types.PizzaRecipeType.RarePizza, 15.99));

        // Depending on whether it's lunch time, add more pizzas
        if (isLunchTime)
        {
            list.Add(new MenuItem("Boo Pizza", Models.Types.PizzaRecipeType.RarePizza, 15.99));
            list.Add(new MenuItem("Baby Mario Pizza", Models.Types.PizzaRecipeType.ChildPizza, 7.99));
        }
        else
        {
            list.Add(new MenuItem("Shy Guy Pizza", Models.Types.PizzaRecipeType.OddPizza, 9.99));
            list.Add(new MenuItem("Bowser Jr Pizza", Models.Types.PizzaRecipeType.ChildPizza, 7.99));
        }

        return list;
    }
}
 