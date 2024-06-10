using AutoMapper;

using ContactsMVC.Models.DTO;

namespace ContactsMVC
{
	public class MappingConfig : Profile
	{
        public MappingConfig()
        {
			
			CreateMap<ContactDTO, ContactCreateDTO>().ReverseMap();
			CreateMap<ContactDTO, ContactUpdateDTO>().ReverseMap();

			
		}
    }
}
