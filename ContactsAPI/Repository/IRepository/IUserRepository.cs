using ContactsAPI.Models.DTO;
using ContactsAPI.Models;

namespace ContactsAPI.Repository.IRepository
{
	public interface IUserRepository
	{
		public bool IsUnique(string username);
		public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
		public Task<LocalUser> Register(RegisterRequestDTO registerRequestDTO);
	}
}
