using Contacts_Utility;
using ContactsMVC.Models;
using ContactsMVC.Models.DTO;
using ContactsMVC.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContactsMVC.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


		[HttpGet]
        public IActionResult Login()
		{
			LoginRequestDTO obj = new();
			return View(obj);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginRequestDTO obj)
		{
			ApiResponse res = await _authService.LoginAsync<ApiResponse>(obj);
			if(res!=null && res.IsSuccess)
			{
				LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(res.Result));
				HttpContext.Session.SetString(SD.SesstionToken, model.Token);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("CustomError",res.ErrorMessages.FirstOrDefault());
				return View(obj);
			}
			return View(obj);
		}

		[HttpGet]
		public IActionResult Register()
		{
			RegisterRequestDTO obj = new();
			return View(obj);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterRequestDTO obj)
		{
			ApiResponse res = await _authService.RegisterAsync<ApiResponse>(obj);
			if (res != null && res.IsSuccess) 
			{
				return RedirectToAction("Login");
			}
			return View(obj);
		}


		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			HttpContext.Session.SetString(SD.SesstionToken, ""); // czyscimy sesje
            return RedirectToAction("Index", "Home");
        }

		[HttpGet]
		public IActionResult AccessDenied()
		{
			
			return View();
		}
	}
}
