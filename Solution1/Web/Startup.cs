using System;
using Business.Services;
using Business.Services.CommunicationServices;
using Business.Services.Interfaces;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<PlatformManagement>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<PlatformManagement>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<PlatformManagement, Guid>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IMarkService, MarkService>();
            services.AddTransient<IMarkRepository, MarkRepository>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IAnouncementRepository, AnouncementRepository>();
            services.AddTransient<IAnouncementService, AnouncementService>();
            services.AddTransient<IHomeworkRepository, HomeworkRepository>();
            services.AddTransient<IHomeworkService, HomeworkService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                       name: "default",
                       template: "{controller=Home}/{action=Index}");
                
                //routes.MapRoute(
                //    "{controller=Mark}/{action=Index}/{id?}",
                //    "{controller=Account}/{action=Login}");
            });

            DatabaseInitializer.RolesSeed(app.ApplicationServices);
        }
    }
}
