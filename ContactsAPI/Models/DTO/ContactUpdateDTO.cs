using static Contacts_Utility.SD;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Models.DTO
{
	public class ContactUpdateDTO
	{
		[Required]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		
		[EmailAddress]
		public string Email { get; set; }

		
		public string Password { get; set; }

		
		[Phone]
		public string PhoneNumber { get; set; }

		
		public DateTime DateOfBirth { get; set; }

		
		public ContactType ContactType { get; set; }
	}
}
