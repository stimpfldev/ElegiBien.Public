using ElegiBien.Web.Models.Contact;
using Microsoft.AspNetCore.Mvc;

namespace ElegiBien.Web.Controllers;

public sealed class ContactController : Controller
{
    private readonly IConfiguration _configuration;

    public ContactController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("/Contact")]
    [HttpGet("/contacto")]
    public IActionResult Index()
    {
        var model = new ContactViewModel
        {
            Email = _configuration["Contact:Email"]
        };

        return View(model);
    }
}
