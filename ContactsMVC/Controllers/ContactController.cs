using AutoMapper;
using ContactsMVC.Models;
using ContactsMVC.Models.DTO;
using ContactsMVC.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContactsMVC.Controllers
{
	public class ContactController : Controller
	{

		private readonly IContactService _contactService;
		private readonly IMapper _mapper;
        public ContactController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
			_mapper = mapper;
        }

        public async Task<IActionResult> IndexContact()
		{
			List<ContactDTO> list = new();
			var response = await _contactService.GetAllAsync<ApiResponse>();
			if(response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<ContactDTO>>(Convert.ToString(response.Result));
			}

			return View(list);
		}

		public async Task<IActionResult> ShowContact(int id)
		{
			var response = await _contactService.GetAsync<ApiResponse>(id);
			if (response != null && response.IsSuccess)
			{
				ContactDTO model = JsonConvert.DeserializeObject<ContactDTO>(Convert.ToString(response.Result));
				return View(model);
			}
			return NotFound();
		}
		public async Task<IActionResult> CreateContact()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateContact(ContactCreateDTO model)
		{
			if (ModelState.IsValid)
			{
				var response = await _contactService.CreateAsync<ApiResponse>(model);
				if (response != null && response.IsSuccess)
				{
					return RedirectToAction(nameof(IndexContact));
				}
			}
			return View(model);
		}

		public async Task<IActionResult> UpdateContact(int id)
		{
            var response = await _contactService.GetAsync<ApiResponse>(id);
            if (response != null && response.IsSuccess)
            {
                ContactDTO model = JsonConvert.DeserializeObject<ContactDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<ContactUpdateDTO>(model));
            }
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContact(ContactUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _contactService.UpdateAsync<ApiResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexContact));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteContact(int id)
        {
            var response = await _contactService.GetAsync<ApiResponse>(id);
            if (response != null && response.IsSuccess)
			{
				ContactDTO model = JsonConvert.DeserializeObject<ContactDTO>(Convert.ToString(response.Result));
				return View(model);
			}
			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteContact(ContactDTO model)
		{

			var response = await _contactService.DeleteAsync<ApiResponse>(model.Id);
			if (response != null && response.IsSuccess)
			{
				return RedirectToAction(nameof(IndexContact));
			}

			return View(model);
		}
	}
}
