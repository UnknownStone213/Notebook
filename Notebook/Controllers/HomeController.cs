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
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;

namespace Notebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IUserService _userService;
        private readonly INoteService _noteService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, INoteService noteService)
        {
            _logger = logger;
			_userService = userService;
            _noteService = noteService;
		}

        public IActionResult Index(string? name, DateTime? date)
        {
            List<User> users = _userService.GetAll();
            IEnumerable<Note> notes = new List<Note> { };
            User? user = null;

            int? UserId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
            if (UserId != null)
            {
                notes = _noteService.GetNotesByUserId(UserId ?? default).Where(x => x.Name.Contains(name ?? "")); // if dont use ?? returns error
                if (date != null)
                {
                    notes = notes.Where(x => x.Date.Date == date);
                }
                user = users.Find(x => x.Id == UserId);
            }

            UserNoteViewModel userNoteViewModel = new UserNoteViewModel 
            {
                Users = users,
                Notes = notes,
                User = user
            };
            return View(userNoteViewModel);
        }

        //[Authorize(Roles = "admin")]
        public IActionResult CreateUser()
        {
            return View();
        }

        //[Authorize(Roles = "admin")]
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

        public IActionResult Edit(int id)
        {
            User? user = _userService.GetUserById(id);
            if (User.FindFirstValue(ClaimTypes.Email) == user.Email || User.FindFirstValue(ClaimTypes.Role) == "admin")
            {
                return View(user);
            }
            else 
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if ((user.Role == "user" || user.Role == "admin") && (User.FindFirstValue(ClaimTypes.Email) == user.Email || User.FindFirstValue(ClaimTypes.Role) == "admin"))
            {
                _userService.Edit(user);
            }
            return RedirectToAction("Index");
        }

        public IActionResult LogIn()
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
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(type: "UserId", value: Convert.ToString(user.Id))
                };

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity));

                return RedirectToAction("Index");
            }

            return RedirectToAction("LogIn");
        }

		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "user")]
        public IActionResult CreateNote() 
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public IActionResult CreateNote(NoteCreateDto noteCreateDto)
        {
            if (ModelState.IsValid)
            {
                noteCreateDto.UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                _noteService.Create(noteCreateDto);
                _logger.Log(LogLevel.None, $"{DateTime.Now} - User {noteCreateDto.UserId} created new note {noteCreateDto.GetInfo()}.", null);

                return RedirectToAction("Index", "Home");
            }
            return View(noteCreateDto);
        }

        [Authorize(Roles = "user")]
        public IActionResult EditNote(int id)
        {
            Note note = _noteService.GetById(id);
            return View(note);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public IActionResult EditNote(Note note)
        {
            _noteService.Edit(note);
            _logger.Log(LogLevel.None, $"{DateTime.Now} - Note {note.GetInfo()} was edited.", null);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public IActionResult DeleteNote(int id)
        {
            _logger.Log(LogLevel.None, $"{DateTime.Now} - Note {_noteService.GetById(id).GetInfo()} was deleted.", null);
            _noteService.DeleteNoteById(id);
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}