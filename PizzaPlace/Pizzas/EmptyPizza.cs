using PizzaPlace.Models.Types;

namespace PizzaPlace.Pizzas;

public record EmptyPizza : Pizza
{
    public EmptyPizza() : base(PizzaRecipeType.EmptyPizza)
    {
    }
}