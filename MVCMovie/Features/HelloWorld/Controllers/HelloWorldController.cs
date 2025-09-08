using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Features.HelloWorld.Controllers;

[Route("hello")]

public class HelloWorldController : Controller
{
    // base URL
    // GET: /hello/
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
    // specifies Welcome URL
    // GET: /hello/welcome/John/3
    // Requires using System.Text.Encodings.Web;
    [HttpGet("welcome/{name?}/{numTimes:int?}")]
    public IActionResult Welcome(string name, int numTimes = 1)
    {
        ViewData["Message"] = "Hello " + name;
        ViewData["NumTimes"] = numTimes;
        return View();
    }
}