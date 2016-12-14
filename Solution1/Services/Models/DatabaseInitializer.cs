using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Services.Models
{
    public class DatabaseInitializer
    {
        private static readonly string[] RolesNames = new string[] { "Admin", "Student", "Professor" };
        
        private static readonly string AdminEmail = "admin@mail.com";
        private static readonly string AdminPassword = "Passw0rd.";

        public static async void RolesSeed(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService(typeof(PlatformManagement)) as PlatformManagement;

            var roleManager = serviceProvider.GetService(typeof(RoleManager<IdentityRole<Guid>>))
                as RoleManager<IdentityRole<Guid>>;

            foreach (var roleName in RolesNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole<Guid>();
                    role.Name = roleName;
                    await roleManager.CreateAsync(role);
                }
            }

            var admin = new ApplicationUser
            {
                Email = AdminEmail,
                UserName = AdminEmail,
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
            context.Dispose();
        }
    }
}
