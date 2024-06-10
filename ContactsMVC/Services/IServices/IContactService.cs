using ContactsMVC.Models.DTO;

namespace ContactsMVC.Services.IServices
{
	public interface IContactService
	{
		Task<T> GetAllAsync<T>();
		Task<T> GetAsync<T>(int id);
		Task<T> DeleteAsync<T>(int id);
		Task<T> CreateAsync<T>(ContactCreateDTO dto);
		Task<T> UpdateAsync<T>(ContactUpdateDTO dto);
		




	}
}
