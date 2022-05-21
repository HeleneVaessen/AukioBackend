using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Services;
using UserService.DAL;
using Microsoft.EntityFrameworkCore;
using UserService.Models;
using Shared;

namespace UserService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IUserDAL, UserDAL>();

            services.AddScoped<IUserService, Services.UserService>();

            services.AddSharedServices("User Service");

            services.AddMessagePublishing("User Service");

            var connection = "Server=userdb;Database=aukio;User=sa;Password=Your_password123;";

            services.AddDbContext<UserContext>(
                 options => options.UseSqlServer(connection));


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserContext context)
        {
            app.UseSharedAppParts("User Service");

            UpdateDatabase(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

 
        public static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                       .GetRequiredService<IServiceScopeFactory>()
                       .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<UserContext>();
            context.Database.Migrate();

            if (context.Users.FirstOrDefault(x => x.Email == "Admin@Admin.com") == null)
            {
                context.Users.Add(new Models.User
                {
                    Name = "Admin",
                    Email = "Admin@Admin.com",
                    School = "Fontys Hogescholen"
                });

                context.SaveChanges();

                using var serviceScope2 = app.ApplicationServices
                       .GetRequiredService<IServiceScopeFactory>()
                       .CreateScope();
            }
        }
    }
}