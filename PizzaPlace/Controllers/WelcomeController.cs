using Microsoft.AspNetCore.Mvc;

namespace PizzaPlace.Controllers;

[Route("api/welcome")]
public class WelcomeController : ControllerBase
{
    // public ActionResult Index()
    // {
    //     //getWelcome();

    //     return new ViewResult() { ViewName = "~/PizzaSolutionFrontend/src/Pages/Welcome.jsx" };
    // }

    [HttpGet]
    public IActionResult Greet()
    {
        Console.WriteLine("Greeted guest.");

        // view Welcome.jsx somehow

        return Ok("Welcome to this super awesome pizza place.");
    }
}
