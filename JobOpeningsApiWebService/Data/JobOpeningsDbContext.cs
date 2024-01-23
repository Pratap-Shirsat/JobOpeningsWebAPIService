using JobOpeningsApiWebService.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace JobOpeningsApiWebService.Data
{
	public class JobOpeningsDbContext : DbContext
	{
		public JobOpeningsDbContext(DbContextOptions<JobOpeningsDbContext> options) : base(options) { }

		public DbSet<Job> Jobs { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData(
					new User { Id = Guid.Parse("cd87c8dd-df68-46cc-a9d5-08dc1852bb58"), Name = "Pratap Shirsat", Username = "Admin", Email = "pratap@admin.com", Phone = "9999999999", UserType = userType.Admin, Password = "$2a$10$VxE26TrbGlxAiapR4dBTq.KCZ0L1r1M032MbWVZ2gdHU9RwQpqMwu" },
					new User { Id = Guid.Parse("22dfa6bc-9368-4271-6a90-08dc18541600"), Name = "Jimin Park", Email = "jimin@admin.com", Phone = "9999999999", Username = "jimin", UserType = userType.Admin, Password = "$2a$10$c2gltoVOEhSIeYtE4ZkctOKSa57ye2i69p1akHwk87Kh8DUHRapM." }
				);
		}
	}
}
