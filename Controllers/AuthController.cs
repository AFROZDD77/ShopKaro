using Microsoft.AspNetCore.Mvc;
using ShopKaro.Models.User;

namespace ShopKaro.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string username, string password)
        {
            // Code to athenticate the user.

            // Code to generate the Token.
            return Redirect("http://localhost:4200");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            return RedirectToAction("LogIn");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return Ok(true);
        }
    }
}
