using Microsoft.AspNetCore.Mvc;
using ShopKaro.Models;
using System.Diagnostics;

namespace ShopKaro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Angular()
        {
            return Redirect("http://localhost:4200");
        }

        public IActionResult WantToSignUp()
        {
            return RedirectToAction("SignUp","Auth");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
