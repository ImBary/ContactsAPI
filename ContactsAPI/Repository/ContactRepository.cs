using ContactsAPI.Data;
using ContactsAPI.Models;
using ContactsAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Repository
{
	public class ContactRepository : Repository<Contact>, IContactRepository
	{
		private readonly ApplicationDbContext _db;

		public ContactRepository(ApplicationDbContext db) : base(db)
        {
			_db = db;
        }

        public async Task<Contact> UpdateAsync(Contact entity)
		{
			
			_db.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}
	}
}
