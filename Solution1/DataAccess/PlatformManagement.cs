using DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace DataAccess
{
    public class PlatformManagement : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        private const string ConnectionString = "User ID=student;Password=student;Host=localhost;Port=5432;Database=solution;Pooling=true;";
        //private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=FiiStudyPlatform;Trusted_Connection=True;MultipleActiveResultSets=true";

        public DbSet<Player> Players { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<Anouncement> Anouncements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
            //optionsBuilder.UseSqlServer(ConnectionString);
            base.OnConfiguring(optionsBuilder);
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Obs: Requires running 'CREATE EXTENSION "uuid-ossp";' on the database
            builder.HasPostgresExtension("uuid-ossp");

            MarkSetUp(builder);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        private void MarkSetUp(ModelBuilder builder)
        {
            builder.Entity<Mark>().HasKey(mark => new { mark.HomeworkId, mark.UserId });
        }

    }
}
