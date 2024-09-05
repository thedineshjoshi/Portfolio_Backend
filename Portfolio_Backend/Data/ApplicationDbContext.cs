using Microsoft.EntityFrameworkCore;
using Portfolio_Backend.Common;
using Portfolio_Backend.Model;
using System.Collections.Generic;
using System.Reflection;

namespace Portfolio_Backend.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Login>Logins { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            
            var login = new Login
            {
                id = Guid.NewGuid(),
                Username = "Dinesh25",
                Password = CommonMethods.ConvertToEncrypt("@Dineshdj@2080")
            };
            builder.Entity<Login>().HasData(login);
        }
    }
}
