using Contacts_Utility;
using ContactsMVC.Models;
using ContactsMVC.Models.DTO;
using ContactsMVC.Services.IServices;
using Microsoft.AspNetCore.Identity.Data;

namespace ContactsMVC.Services
{
	public class AuthService : BaseService, IAuthService
	{
		private readonly IHttpClientFactory _clientFactory;
		private string contactUrl;
		public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
		{
			_clientFactory = clientFactory;
			contactUrl = configuration.GetValue<string>("ServiceUrls:ContactAPI");

		}

		public Task<T> LoginAsync<T>(LoginRequestDTO loginRequest)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = loginRequest,
				Url = contactUrl + "/api/UsersAuth/login"
			});
		}

		public Task<T> RegisterAsync<T>(RegisterRequestDTO registerRequest)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = registerRequest,
				Url = contactUrl + "/api/UsersAuth/register"
			});
		}
	}
}
