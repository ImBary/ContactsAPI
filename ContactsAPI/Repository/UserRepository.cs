using ContactsAPI.Data;
using ContactsAPI.Models;
using ContactsAPI.Models.DTO;
using ContactsAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ContactsAPI.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _db;
		private string secretKey;
        public UserRepository(ApplicationDbContext db, IConfiguration _config)
        {
            _db = db;
			secretKey = _config.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUnique(string username)
		{
			var user = _db.LocalUsers.FirstOrDefault(u=>u.UserName == username);
			if (user == null) {
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			var user = _db.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
			&& u.Passsword == loginRequestDTO.Password);
			if (user == null)
			{
				return null;
			}

			//if user was found generate JWT Token

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id.ToString()),
					new Claim(ClaimTypes.Role, user.Role)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
			{
				Token = tokenHandler.WriteToken(token),
				User = user
			};
			return loginResponseDTO;
		}

		public async Task<LocalUser> Register(RegisterRequestDTO registerRequestDTO)
		{
			LocalUser user = new LocalUser()
			{
				UserName = registerRequestDTO.UserName,
				Name = registerRequestDTO.Name,
				Passsword = registerRequestDTO.Passsword,
				Role = registerRequestDTO.Role
			};
			await _db.LocalUsers.AddAsync(user);
			await _db.SaveChangesAsync();
			user.Passsword = "";
			return user;
		}
	}
}
