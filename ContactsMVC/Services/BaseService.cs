using Contacts_Utility;
using ContactsMVC.Models;
using ContactsMVC.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ContactsMVC.Services
{
	public class BaseService : IBaseService
	{
		public ApiResponse responseModel { get; set; }
		public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
			this.responseModel = new();
			this.httpClient = httpClient;
            
        }

		  

		public async Task<T> SendAsync<T>(ApiRequest apiRequest)
		{
			try
			{
				var client = httpClient.CreateClient("ContactClient");//tworzymy nowego clienta do zapytania
				HttpRequestMessage message = new HttpRequestMessage();
				message.Headers.Add("Accept", "application/json");//typ danych
				message.RequestUri = new Uri(apiRequest.Url);
				if (apiRequest.Data != null)//sprawdzamy czy w zapytaniu sa jakies dane jak tak to je serializujemy 
				{
					message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),Encoding.UTF8,"application/json");
				}

				switch (apiRequest.ApiType)//okreslamy typ http zapytania
				{
					case SD.ApiType.PUT:
						message.Method = HttpMethod.Put;
						break;
					case SD.ApiType.DELETE:
						message.Method = HttpMethod.Delete;
						break;
					case SD.ApiType.POST:
						message.Method = HttpMethod.Post;
						break;
					default:
						message.Method = HttpMethod.Get;
						break;
				}

				HttpResponseMessage apiResponse = null;
				apiResponse = await client.SendAsync(message); //czekamy na odpowiedz 

				var apiContent = await apiResponse.Content.ReadAsStringAsync();//zwracamy odpowiedz
				var ApiResponse = JsonConvert.DeserializeObject<T>(apiContent);
				return ApiResponse;


			}
			catch(Exception ex)
			{
				var dto = new ApiResponse
				{
					ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
					IsSuccess = false
				};
				var res = JsonConvert.SerializeObject(dto);
				var APIResponse = JsonConvert.DeserializeObject<T>(res);
				return APIResponse;

			}


		}
	}
}
