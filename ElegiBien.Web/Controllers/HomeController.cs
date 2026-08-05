using Microsoft.AspNetCore.Mvc;

namespace ElegiBien.Web.Controllers;

public sealed class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
