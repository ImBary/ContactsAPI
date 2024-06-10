using Contacts_Utility;
using ContactsMVC.Models;
using ContactsMVC.Models.DTO;
using ContactsMVC.Services.IServices;

namespace ContactsMVC.Services
{
	public class ContactService : BaseService, IContactService
	{
		private readonly IHttpClientFactory _clientFactory;
		private string contactUrl;
		public ContactService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
		{
			_clientFactory = clientFactory;
			contactUrl = configuration.GetValue<string>("ServiceUrls:ContactAPI");

		}
		public Task<T> CreateAsync<T>(ContactCreateDTO dto)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = dto,
				Url = contactUrl + "/api/ContactApi"
			});
		}

		public Task<T> DeleteAsync<T>(int id)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = contactUrl + "/api/ContactApi/"+id
			});
		}

		public Task<T> GetAllAsync<T>()
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType= SD.ApiType.GET,
				Url= contactUrl+ "/api/ContactApi"
			});
		}

		public Task<T> GetAsync<T>(int id)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = contactUrl + "/api/ContactApi/" + id
			});
		}

		public Task<T> UpdateAsync<T>(ContactUpdateDTO dto)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.PUT,
				Data = dto,
				Url = contactUrl + "/api/ContactApi/"+dto.Id
			});
		}
	}
}
