using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Notebook.BusinessLogic.Interfaces;

namespace Notebook.Controllers
{
    public class HomeController : Controller
    {
        //ApplicationContext db;
        private readonly ILogger<HomeController> _logger;
		private readonly IUserService _userService;

		public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
			_userService = userService;
		}

        public async Task<IActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid && (user.Role == "user" || user.Role == "admin"))
            {
				db.Users.Add(user);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User? user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User? user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null) return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (user.Role == "user" || user.Role == "admin")
            {
				db.Users.Update(user);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
            return View(user);
        }

        //public async Task<IActionResult> LogIn(UserLogInDto user) 
        //{

        //}

		[Authorize]
		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Index", "Home");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}