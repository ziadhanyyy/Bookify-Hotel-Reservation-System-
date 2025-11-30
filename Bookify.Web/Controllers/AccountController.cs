using Bookify.Core.DTOs;
using Bookify.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Bookify.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;


        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpGet]
        public IActionResult Register() => View(new RegisterDto() { Role = "User" });


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);


            var result = await _authService.RegisterAsync(dto);


            if (!result.Succeeded)
            {
                foreach (var err in result.Errors) { 
                    ModelState.AddModelError(string.Empty, err.Description);

                Console.WriteLine(err.Description);
            }
                return View(dto);
            }

            TempData["Success"] = "Account created successfully! Please login.";
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Login() => View(new LoginDto());


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);


            var result = await _authService.LoginAsync(dto);


            if (result.Succeeded)
            {
                // Check if user is Admin and redirect to Admin Dashboard
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                
                // Regular users go to Home page
                return RedirectToAction("Index", "Home");
            }


            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(dto);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}