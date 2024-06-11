using AutoMapper;
using ContactsMVC.Models;
using ContactsMVC.Models.DTO;
using ContactsMVC.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ContactsMVC.Controllers
{
	public class HomeController : Controller
	{
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        public HomeController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
		{
            List<ContactDTO> list = new();
            var response = await _contactService.GetAllAsync<ApiResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ContactDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

		

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
