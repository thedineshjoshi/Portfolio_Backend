using Microsoft.EntityFrameworkCore;
using Portfolio_Backend.Common;
using Portfolio_Backend.Model;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Label = Portfolio_Backend.Model.Label;

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
        public DbSet<Label> Labels { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BlogLabel> BlogLabels { get; set; }

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

            builder.Entity<Label>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });


            builder.Entity<BlogLabel>(entity =>
            {
                entity.HasKey(nl => new { nl.BlogId, nl.LabelId });
                entity.HasOne(nl => nl.Label)
                      .WithMany(l => l.BlogLabels)
                      .HasForeignKey(nl => nl.LabelId);
            });
            builder.Entity<Blog>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.HasMany(b => b.Comments)
                      .WithOne()
                      .HasForeignKey(c => c.BlogId)
                      .OnDelete(DeleteBehavior.Cascade); // Optional: defines cascading delete behavior
            });

            // Configure Comment entity
            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne<Blog>()
                      .WithMany(b => b.Comments)
                      .HasForeignKey(c => c.BlogId)
                      .OnDelete(DeleteBehavior.Cascade); // Optional: defines cascading delete behavior
            });

        }
    }
}
