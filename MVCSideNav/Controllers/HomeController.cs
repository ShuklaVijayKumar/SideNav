using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCSideNav.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace MVCSideNav.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Login(string name)
        {
            if (string.IsNullOrEmpty(name))
               return RedirectToAction(nameof(Index));

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, "admin")
            },CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme, principal);

           return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "MustBeAdmin")]
        public IActionResult Manage() => View();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ErrorForbidden() => View();

        public IActionResult ErrorNotLoggedIn() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
