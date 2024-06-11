using ContactsAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Contacts_Utility.SD;

namespace ContactsAPI.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		
		public DbSet<Contact> Contacts { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Contact>().HasData(
				new Contact
				{
					Id = 1,
					Name = "John Doe",
					Email = "john.doe@example.com",
					Password = "securepassword123",
					PhoneNumber = "+1234567890",
					DateOfBirth = new DateTime(1990, 1, 1),
					ContactType = "Personal"
				});
		}
	}
}
