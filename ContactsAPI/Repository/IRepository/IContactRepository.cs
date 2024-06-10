using ContactsAPI.Models;

namespace ContactsAPI.Repository.IRepository
{
	public interface IContactRepository : IRepository<Contact>
	{
		Task <Contact> UpdateAsync(Contact entity);
		
	}
}
