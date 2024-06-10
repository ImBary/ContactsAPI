using ContactsMVC.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ContactsMVC.Services.IServices
{
	public interface IBaseService
	{
		ApiResponse responseModel { get; set; }
		Task<T> SendAsync<T>(ApiRequest apiRequest);
	}
}
