using AutoMapper;
using ContactsAPI.Data;
using ContactsAPI.Models;
using ContactsAPI.Models.DTO;
using ContactsAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
		private readonly IMapper _mapper;
		
        public UserRepository(IMapper mapper ,ApplicationDbContext db, IConfiguration _config, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
			secretKey = _config.GetValue<string>("ApiSettings:Secret");
			_userManager = userManager;
			_mapper = mapper;
			_roleManager = roleManager;
        }
        public bool IsUnique(string username)
		{
			var user = _db.ApplicationUsers.FirstOrDefault(u=>u.UserName == username);
			if (user == null) {
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
			
			bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

			
			if (user == null || isValid==false)
			{
				return new LoginResponseDTO()
				{
					Token = "",
					User = null
				};
			}

			//if user was found generate JWT Token
			var role = await _userManager.GetRolesAsync(user);
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.UserName.ToString()),
					new Claim(ClaimTypes.Role, role.FirstOrDefault())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
			{
				Token = tokenHandler.WriteToken(token),
				User = _mapper.Map<UserDTO>(user),
				
			};
			return loginResponseDTO;
		}

		public async Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO)
		{
			ApplicationUser user = new()
			{
				Name = registerRequestDTO.Name,
				Email = registerRequestDTO.UserName,
				NormalizedEmail = registerRequestDTO.UserName.ToUpper(),
				UserName = registerRequestDTO.UserName,
				
			};
			try
			{
				var res = await _userManager.CreateAsync(user, registerRequestDTO.Passsword);
				if (res.Succeeded)
				{
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
					{
						await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("user"));
                    }
					await _userManager.AddToRoleAsync(user, "admin");
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registerRequestDTO.UserName);
                    return _mapper.Map<UserDTO>(userToReturn);
                }
                
                return new UserDTO();

            }
            catch(Exception ex)
			{
                return new UserDTO();
            }

            

        }
	}
}
