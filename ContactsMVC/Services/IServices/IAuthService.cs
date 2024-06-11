using ContactsMVC.Models.DTO;
using Microsoft.AspNetCore.Identity.Data;

namespace ContactsMVC.Services.IServices
{
	public interface IAuthService
	{
		Task<T> LoginAsync<T>(LoginRequestDTO loginRequest);
		Task<T> RegisterAsync<T>(RegisterRequestDTO registerRequest);

	}
}
