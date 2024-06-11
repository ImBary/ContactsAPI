using AutoMapper;
using ContactsAPI.Models;
using ContactsAPI.Models.DTO;

namespace ContactsAPI
{
	public class MappingConfig : Profile
	{
        public MappingConfig()
        {
            CreateMap<Contact, ContactDTO>();
			CreateMap<Contact, ContactCreateDTO>().ReverseMap();
			CreateMap<Contact, ContactUpdateDTO>().ReverseMap();

			CreateMap<ContactDTO,Contact>();
			CreateMap<ApplicationUser, UserDTO>().ReverseMap();
		}
    }
}
