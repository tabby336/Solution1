using System;
using System.IO;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Business.Services
{
    public class DatabaseInitializer
    {
        private static readonly string[] RolesNames = new string[] {"Admin", "Student", "Professor"};

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
                Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
            };

            var userManager = serviceProvider.GetService(typeof(UserManager<ApplicationUser>))
                as UserManager<ApplicationUser>;

            var user = userManager.FindByIdAsync(admin.Id.ToString()).Result;

            if (user == null)
            {
                await userManager.CreateAsync(admin, AdminPassword);
                await userManager.AddToRoleAsync(admin, "Admin");
                await userManager.AddToRoleAsync(admin, "Professor");
                await userManager.AddToRoleAsync(admin, "Student");
            }

            await context.SaveChangesAsync();

            // insert sample data here
            InsertAdminSample(serviceProvider);
            await InsertStudentSample(serviceProvider);
            await InsertProfessorSample(serviceProvider);
            InsertCourseSamples(serviceProvider);
            InsertMarkSamples(serviceProvider);
            InsertHomeworkSamples(serviceProvider);
            InsertSampleAnouncements(serviceProvider);
            context.Dispose();
        }

        public static void InsertAdminSample(IServiceProvider serviceProvider)
        {

            try
            {
                var playerRepository = serviceProvider.GetService(typeof(IPlayerRepository)) as IPlayerRepository;

                //add admin player 
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
            }
            catch
            {
            }
        }

        public static async Task InsertStudentSample(IServiceProvider serviceProvider)
        {

            try
            {
                var playerRepository = serviceProvider.GetService(typeof(IPlayerRepository)) as IPlayerRepository;
                var userManager = serviceProvider.GetService(typeof(UserManager<ApplicationUser>))
                    as UserManager<ApplicationUser>;

                //add student
                var student = new ApplicationUser
                {
                    Email = "student@fii.com",
                    UserName = "student",
                    Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aef")
                };

                await userManager.CreateAsync(student, "Student0.");
                await userManager.AddToRoleAsync(student, "Student");

                var s = new Player()
                {
                    Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aef")
                    ,
                    CollegeId = "6968"
                    ,
                    DateOfBirth = DateTime.Parse("09/09/1992")
                    ,
                    FirstName = "Student"
                    ,
                    LastName = "Valoros"
                    ,
                    Semester = 5
                };
                playerRepository.Create(s);
            }
            catch
            {
            }
        }

        public static async Task InsertProfessorSample(IServiceProvider serviceProvider)
        {

            try
            {
                var playerRepository = serviceProvider.GetService(typeof(IPlayerRepository)) as IPlayerRepository;
                var userManager = serviceProvider.GetService(typeof(UserManager<ApplicationUser>))
                    as UserManager<ApplicationUser>;

                //add professor
                var professor = new ApplicationUser
                {
                    Email = "prof@fii.com",
                    UserName = "profu",
                    Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aeb")
                };

                await userManager.CreateAsync(professor, "Professor0.");
                await userManager.AddToRoleAsync(professor, "Professor");

                var t = new Player()
                {
                    Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aeb")
                    ,
                    CollegeId = "6967"
                    ,
                    DateOfBirth = DateTime.Parse("09/09/1982")
                    ,
                    FirstName = "Professor"
                    ,
                    LastName = "Valuable"
                    ,
                    Semester = 5
                };
                playerRepository.Create(t);
            }
            catch
            {
            }
        }


        public static void InsertCourseSamples(IServiceProvider serviceProvider)
        {
            try
            {
                var courseRepository = serviceProvider.GetService(typeof(ICourseRepository)) as ICourseRepository;
                var playerCourseRepository =
                    serviceProvider.GetService(typeof(IPlayerCourseRepository)) as IPlayerCourseRepository;

                //add course
                var c = new Course()
                {
                    Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
                    ,
                    Author = "Marian FX"
                    ,
                    Description = "This is only a test course."
                    ,
                    HashTag = "valoare"
                    ,
                    Title = "Fx Test Course"
                };
                //add modules to course
                var module1 = new Module()
                {
                    Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
                    ,
                    Description = "The first ever module of the application."
                    ,
                    Title = "Awesome First Module."
                };
                var module2 = new Module()
                {
                    Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aef")
                    ,
                    Description = "The second ever module of the application."
                    ,
                    Title = "Awesome second module."
                };
                c.Modules.Add(module1);
                c.Modules.Add(module2);

                courseRepository.Create(c);


                // assign course to teacher
                var playerCourse = new PlayerCourse()
                {
                    PlayerId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aeb"),
                    CourseId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
                };
                playerCourseRepository.Create(playerCourse);
            }
            catch
            {
            }   
        }

        public static void InsertHomeworkSamples(IServiceProvider serviceProvider)
        {
            try
            {
                string root = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, 
                                                "Web", "Data", "homeworks");
                var homeworkRepository = serviceProvider.GetService(typeof(IHomeworkRepository)) as IHomeworkRepository;
                var hw1 = new Homework()
                {
                    ModuleId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
                    ,
                    Observations = ""
                    ,
                    OwesMeMoney = false
                    ,
                    Timestamp = DateTime.Now 
                    ,
                    Url = Path.Combine(root, "bade8051-f56d-4187-9726-8694c9ca6aee",
                                "bade8051-f56d-4187-9726-8694c9ca6aef",  "bex3.c")
                    ,
                    UserId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aef")
                };
                var hw2 = new Homework()
                {
                    ModuleId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aef")
                    ,
                    Observations = ""
                    ,
                    OwesMeMoney = false
                    ,
                    Timestamp = DateTime.Now
                    ,
                    Url = Path.Combine(root, "bade8051-f56d-4187-9726-8694c9ca6aef",
                                "bade8051-f56d-4187-9726-8694c9ca6aef", "fex3.c")
                    ,
                    UserId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aef")
                };
                homeworkRepository.Create(hw1);
                homeworkRepository.Create(hw2);
            }
            catch
            {
            }
        }

        public static void InsertMarkSamples(IServiceProvider serviceProvider)
        {
            try
            {
                var markRepository = serviceProvider.GetService(typeof(IMarkRepository)) as IMarkRepository;
                var mark1 = new Mark()
                {
                    HomeworkId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee")
                    ,
                    UserId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aef")
                    ,
                    CreatorId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aeb")
                    ,
                    Description = ""
                    ,
                    HasComment = false
                    ,
                    HasContestation = false
                    ,
                    Id = Guid.Parse("abde8051-f56d-4187-9726-8694c9ca6aef")
                    ,
                    Timestamp = DateTime.Now
                    ,
                    Value = 10
                };

                var mark2 = new Mark()
                {
                    HomeworkId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aef")
                    ,
                    UserId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aef")
                    ,
                    CreatorId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aeb")
                    ,
                    Description = "la"
                    ,
                    HasComment = false
                    ,
                    HasContestation = false
                    ,
                    Id = Guid.Parse("aade8051-f56d-4187-9726-8694c9ca6aef")
                    ,
                    Timestamp = DateTime.Now
                    ,
                    Value = 7
                };
                markRepository.Create(mark1);
                markRepository.Create(mark2);
            }
            catch
            {
            }
        }
            
        public static void InsertSampleAnouncements(IServiceProvider serviceProvider)
        {
            var anouncementRepository = serviceProvider.GetService(typeof(IAnouncementRepository)) as IAnouncementRepository;
            var anouncement1 = new Anouncement
            {
                Id = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee"),
                CourseId = Guid.Parse("bade8051-f56d-4187-9726-8694c9ca6aee"),
                Date = DateTime.Now,
                Text = "We announce you that during the day of 24.12.2017, the PI course will not take place! Have a nice evening.",
                Title = "Course cancelled!"
            };

            try
            {
                anouncementRepository.Create(anouncement1);
            }
            catch
            {
                
            }
        }
    }
}
