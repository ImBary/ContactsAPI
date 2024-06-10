using AutoMapper;
using ContactsAPI.Models;
using ContactsAPI.Models.DTO;
using ContactsAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ContactsAPI.Controllers
{
	[Route("api/ContactApi")]
	[ApiController]
	public class ContactController : Controller
	{

		protected ApiResponse _res;
		private readonly IContactRepository _dbContact;
		private readonly IMapper _mapper;

		public ContactController(IContactRepository dbContact, IMapper mapper)
		{
			_mapper = mapper;
			_dbContact = dbContact;
			this._res = new();

		}

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> GetContacts()//wszystkie kontakty
		{
			try
			{
				IEnumerable<Contact> contactList = await _dbContact.GetAllAsync(); // pobranie kontaktow z przypisaniem do result 
				_res.Result = _mapper.Map<List<ContactDTO>>(contactList);
				_res.StatusCode = HttpStatusCode.OK;
				return Ok(_res);


			}
			catch (Exception ex)
			{
				_res.IsSuccess = false;
				_res.StatusCode = HttpStatusCode.BadRequest;
				_res.ErrorMessages = new List<string>() { ex.Message.ToString() };
				return BadRequest(_res);

			}

		}

		[HttpGet("{id:int}", Name = "GetContact")]
		//[Authorize]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> GetContactById(int id)
		{
			try
			{
				if (id <= 0)
				{
					_res.IsSuccess = false;
					_res.StatusCode = HttpStatusCode.BadRequest;
					_res.ErrorMessages = new List<string>() { "Invalid ID" };
					return BadRequest(_res);
				}
				var contact = await _dbContact.GetAsync(u => u.Id == id);
				if (contact == null)
				{
					_res.IsSuccess = false;
					_res.StatusCode = HttpStatusCode.NotFound;
					_res.ErrorMessages = new List<string>() { "User Not Found" };
					return NotFound(_res);
				}
				_res.StatusCode = HttpStatusCode.OK;
				_res.Result = _mapper.Map<ContactDTO>(contact);
				return Ok(_res);

			}
			catch (Exception ex)
			{
				_res.IsSuccess = false;
				_res.StatusCode = HttpStatusCode.BadRequest;
				_res.ErrorMessages = new List<string>() { ex.Message.ToString() };
				return BadRequest(_res);

			}

		}


		[HttpPost]
		[ProducesResponseType(201)]
		[ProducesResponseType(404)]
		[ProducesResponseType(400)]
		//[Authorize(Roles = "admin")]
		public async Task<ActionResult<ApiResponse>> CreateContact([FromBody] ContactCreateDTO contactCreateDto)
		{
			try
			{
				if (contactCreateDto == null)
				{
					_res.StatusCode = HttpStatusCode.BadRequest;
					_res.IsSuccess = false;
					_res.ErrorMessages = new List<string>() { "Contact is Null" };
					return BadRequest(_res);
				}

				if (await _dbContact.GetAsync(u => u.Email.ToLower() == contactCreateDto.Email.ToLower()) != null)//check czy email jest uniq
				{
					_res.IsSuccess = false;
					_res.StatusCode = HttpStatusCode.BadRequest;
					_res.ErrorMessages = new List<string>() { "Email already in Base" };
					ModelState.AddModelError("", "Email must be unique");
					return BadRequest(_res);
				}

				Contact contact = _mapper.Map<Contact>(contactCreateDto);
				await _dbContact.CreateAsync(contact);
				_res.Result = _mapper.Map<ContactDTO>(contact);
				_res.StatusCode = HttpStatusCode.OK;
				return CreatedAtRoute("GetContact", new { id = contact.Id }, _res);

			}
			catch (Exception ex)
			{
				_res.IsSuccess = false;
				_res.StatusCode = HttpStatusCode.BadRequest;
				_res.ErrorMessages = new List<string>() { ex.Message.ToString() };
				return BadRequest(_res);

			}
		}

		[HttpDelete("{id:int}", Name = "DeleteContact")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<ApiResponse>> DeleteContact(int id)
		{
			try
			{
				//sprawdzamy czy istnieje i czy id jest poprawne -> usun usera
				if (id <= 0)
				{
					_res.IsSuccess = false;
					_res.StatusCode = HttpStatusCode.BadRequest;
					_res.ErrorMessages = new List<string>() { "Invalid ID" };
					return BadRequest(_res);
				}
				var contact = await _dbContact.GetAsync(u => u.Id == id);
				if (contact == null)
				{
					_res.IsSuccess = false;
					_res.StatusCode = HttpStatusCode.NotFound;
					_res.ErrorMessages = new List<string>() { "User Not Found" };
					return NotFound(_res);
				}

				await _dbContact.RemoveAsync(contact);
				_res.IsSuccess = true;
				_res.StatusCode = HttpStatusCode.NoContent;
				return Ok(_res);

			}
			catch (Exception ex)
			{
				_res.IsSuccess = false;
				_res.StatusCode = HttpStatusCode.BadRequest;
				_res.ErrorMessages = new List<string>() { ex.Message.ToString() };
				return BadRequest(_res);

			}
		}
		[HttpPut("{id:int}", Name = "UpdateContact")]
		//[Authorize(Roles = "Admin")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<ApiResponse>> UpdateContact(int id, [FromBody] ContactUpdateDTO upadteDTO)
		{
			try
			{
				if (upadteDTO == null || id != upadteDTO.Id)
				{
					_res.IsSuccess = false;
					_res.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_res);
				}
				Contact model = _mapper.Map<Contact>(upadteDTO);

				await _dbContact.UpdateAsync(model);
				_res.IsSuccess = true;
				_res.StatusCode = HttpStatusCode.NoContent;
				return Ok(_res);
			}
			catch (Exception ex)
			{
				_res.IsSuccess = false;
				_res.ErrorMessages = new List<string>() { ex.ToString() };
				return _res;
			}
		}
	}
}
