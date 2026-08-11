using Microsoft.AspNetCore.Mvc;

namespace ElegiBien.Web.Controllers;

public class LegalController : Controller
{
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Terms()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Methodology()
    {
        return View();
    }
}