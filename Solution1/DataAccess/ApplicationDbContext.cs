using Microsoft.EntityFrameworkCore;
using DataAccess;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Services.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Mark> Marks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "User ID=student;Password=student;Host=localhost;Port=5432;Database=proiect;Pooling=true;";
            optionsBuilder.UseNpgsql(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("uuid-ossp");

            markSetUp(builder);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        private void markSetUp(ModelBuilder builder)
        {
            builder.Entity<Mark>().HasKey(mark => new { mark.HomeworkId, mark.StudentId });
        }
    }
}
