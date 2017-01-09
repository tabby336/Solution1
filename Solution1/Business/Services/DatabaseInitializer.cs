using System;
using System.Collections.Generic;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Business.Services
{
    public class DatabaseInitializer
    {
        private static readonly string[] RolesNames = new string[] { "Admin", "Student", "Professor" };

        private const string AdminEmail = "admin@mail.com";
        private const string AdminUsername = "admin";
        private const string AdminPassword = "Passw0rd.";

        public static async void RolesSeed(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService(typeof(PlatformManagement)) as PlatformManagement;

            var roleManager = serviceProvider.GetService(typeof(RoleManager<IdentityRole<Guid>>))
                as RoleManager<IdentityRole<Guid>>;

            foreach (var roleName in RolesNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole<Guid> {Name = roleName};
                    await roleManager.CreateAsync(role);
                }
            }

            var admin = new ApplicationUser
            {
                Email = AdminEmail,
                UserName = AdminUsername,
                Id = new Guid()
            };

            var userManager = serviceProvider.GetService(typeof(UserManager<ApplicationUser>))
                as UserManager<ApplicationUser>;
            
            var user = userManager.FindByEmailAsync(AdminEmail).Result;

            if (user == null)
            {
                await userManager.CreateAsync(admin, AdminPassword);
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            await context.SaveChangesAsync();

            // insert sample data here
            InsertSamplesIntoTheDatabase(serviceProvider);
            context.Dispose();
        }

        public static void InsertSamplesIntoTheDatabase(IServiceProvider serviceProvider)
        {
            var courseRepository = serviceProvider.GetService(typeof(ICourseRepository)) as ICourseRepository;
            var playerRepository = serviceProvider.GetService(typeof(IPlayerRepository)) as IPlayerRepository;
           
            try
            {
                //add player
                var p = new Player()
                {
                    Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
                    ,
                    CollegeId = "6969"
                    ,
                    DateOfBirth = DateTime.Parse("09/09/1996")
                    ,
                    FirstName = "Arwen"
                    ,
                    LastName = "Eowin"
                    ,
                    Semester = 5
                };
                playerRepository.Create(p);

                //add course
                var c = new Course()
                {
                    Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
                    ,
                    Author = "Marian FX"
                    ,
                    Description = "This is only a test course."
                    ,
                    HashTag = "#valoare"
                    ,
                    Title = "Fx Test Course"
                };
                //add modules to course
                var module1 = new Module()
                {
                    //CourseId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
                    //,
                    Description = "The first ever module of the application."
                    ,
                    Title = "No title provided."
                };
                var module2 = new Module()
                {
                    //CourseId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
                    //,
                    Description = "The second ever module of the application."
                    ,
                    Title = "No title provided."
                };
                c.Modules.Add(module1);
                c.Modules.Add(module2);

                courseRepository.Create(c);
            }
            catch
            {
            }
        }
    }
}
