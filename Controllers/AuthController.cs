using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopKaro.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopKaro.Controllers
{
    public class AuthController : Controller
    {

        private string GenerateJwtToken(string userName, string role)
        {
            var key = Encoding.ASCII.GetBytes("f3f5cbac3d4d1ade80eef71db4324270c83f229c73614fe4f2d1d51ca157fb6a5c385e8078b2beab24fda4d83dc4772e09f2d350125115edb99ee43846590b3d17818f7d27d4b43d5419820877f425b5295e21999e6f76e20c719bda3a0e479de5692b237ff805ccbead61be96bdc2d414f5ec34e131947f14b56f06267fa74d0cdc5a709f65178fb660f6d0285cda0cb25da143f3b634ac267736d85499a5bbd923b6452fa7930460f98834a2cacfd805cfa34006a4a27dba9ad50d5d5c5045b0670a6462e5f8405c4c067a74c0ad60edab1516ed5a10bdfc12d514af417be14f4566308674fa4abc961cbce8abd5c987539b3fbeb8b67ecb2c29414c2ad30b20ca0ad924d7eca3d68380909f5c4b04714b989529f0594cfad15b277d22d3e7"); // Match the key used in Program.cs
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, userName), // User name
            new Claim(ClaimTypes.Role, role) // Role for role-based authorization
        }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                Issuer = "ShopKaro", // Match with ValidIssuer in Program.cs
                Audience = "ShopKaro", // Match with ValidAudience in Program.cs
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Returns the JWT token as a string
        }


        private readonly UserDbContext _userDbcontex;
        private readonly IConfiguration _configuration;
        public AuthController(UserDbContext userDbContext, IConfiguration configuration) 
        {
            _userDbcontex = userDbContext;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string username, string password)
        {
            var user = _userDbcontex.Users.FirstOrDefault(u => u.UserName == username);

            //if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            //{
            //    ViewBag.Error = "Invalid email or password.";
            //    return View();
            //}

            // Generate JWT token
            var token = GenerateJwtToken(user.UserName, user.Role);

            // Return the token
            //_configuration.Bind("JWTtoken");
            // return Ok(new { Token = token });
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (_userDbcontex.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(user);
                }

                // Hash the password
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // Set default role for new users
                user.Role = "User";

                // Add the user to the database
                _userDbcontex.Users.Add(user);
                _userDbcontex.SaveChanges();

                // Redirect to login page after successful sign-up
                return RedirectToAction("Login");
            }

            return View(user);
        }
    }
}
