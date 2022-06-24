using Banking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Banking.Data
{
	public class ApplicationDBContext : IdentityDbContext
	{
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
		: base(options)
		{
		}

		//public DbSet<User> User { get; set; }
		public DbSet<ApplicationUser> ApplicationUser { get; set; }
		public DbSet<Transaction> Transaction { get; set; }

		public ApplicationDBContext()
		{
		}
	}
}
