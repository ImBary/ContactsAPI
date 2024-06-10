using static Contacts_Utility.SD;
using System.ComponentModel.DataAnnotations;

namespace ContactsMVC.Models.DTO
{
	public class ContactCreateDTO
	{
		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100)]
		public string Password { get; set; }

		[Required]
		[Phone]
		public string PhoneNumber { get; set; }

		[Required]
		public DateTime DateOfBirth { get; set; }

		[Required]
		public ContactType ContactType { get; set; }
	}
}
