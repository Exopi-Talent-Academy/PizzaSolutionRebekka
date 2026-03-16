using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/menu")]
public class MenuController(TimeProvider timeProvider, IMenuService menuService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetMenu()
    {
        return Ok(menuService.GetMenu(timeProvider.GetUtcNow()));

        int currentTime = timeProvider.GetUtcNow().Hour;

        if (currentTime >= 11 && currentTime < 14 )
        {
            // Return the lunch menu
        }
        else
        {
            // Return the standard menu
        }
    }
}
