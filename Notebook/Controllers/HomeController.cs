using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Notebook.BusinessLogic.Interfaces;
using Notebook.Common.Dto;
using System.Security.Claims;

namespace Notebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IUserService _userService;

		public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
			_userService = userService;
		}

        public IActionResult Index()
        {
            return View( _userService.GetAll());
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(UserCreateDto userCreateDto)
        {
            if (ModelState.IsValid && (userCreateDto.Role == "user" || userCreateDto.Role == "admin"))
            {
				_userService.Create(userCreateDto);

				return RedirectToAction("Index", "Home");
			}
            return View(userCreateDto);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
			User? user = _userService.GetUserById(id);
			if (user != null)
			{
				_userService.DeleteUserById(id);
				return RedirectToAction("Index", "Home");
			}
            else 
                return NotFound();
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id != null)
        //    {
        //        User? user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
        //        if (user != null) return View(user);
        //    }
        //    return NotFound();
        //}

        //     [HttpPost]
        //     public async Task<IActionResult> Edit(User user)
        //     {
        //         if (user.Role == "user" || user.Role == "admin")
        //         {
        //	db.Users.Update(user);
        //	await db.SaveChangesAsync();
        //	return RedirectToAction("Index");
        //}
        //         return View(user);
        //     }

        public async Task<IActionResult> LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(Common.Dto.UserLogInDto userLogInDto)
        {
            var user = _userService.Get(userLogInDto);

            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Email)
                };

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity));

                _logger.LogInformation($"{DateTime.Now}: user with {userLogInDto.Email} has logged in");

                return RedirectToAction("Index");
            }

            return RedirectToAction("LogIn");
        }

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