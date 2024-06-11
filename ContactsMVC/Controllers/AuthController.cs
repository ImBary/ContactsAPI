using Contacts_Utility;
using ContactsMVC.Models;
using ContactsMVC.Models.DTO;
using ContactsMVC.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(model.Token);

				//setting calims so server would know if user is login
				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
				identity.AddClaim(new Claim(ClaimTypes.Name, model.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u=>u.Type=="role").Value));
				var principal = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);

                HttpContext.Session.SetString(SD.SesstionToken, model.Token);//setting session
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("CustomError",res.ErrorMessages.FirstOrDefault());
				return View(obj);
			}
			
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
            if (res != null && res.ErrorMessages != null && res.ErrorMessages.Any())
            {
                foreach (var error in res.ErrorMessages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
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
