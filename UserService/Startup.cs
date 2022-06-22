using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.Consul;
using System.Text;
using UserService.DAL;
using UserService.Models;
using UserService.Services;

namespace UserService
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            var JWTKey = "JWTKeyForAukioCreatedIn2022";
            var connection = "Server=aukiodb;Database=userdb;User=sa;Password=Your_password123;";

            serviceCollection.AddAuthentication(x =>
            {
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false

                };
            });


            serviceCollection.AddControllers();

            ConfigureConsul(serviceCollection);

            serviceCollection.AddAuthorization();

            serviceCollection.AddScoped<IUserDAL, UserDAL>();

            serviceCollection.AddScoped<IUserService, Services.UserService>();

            serviceCollection.AddSharedServices("User Service");

            serviceCollection.AddMessagePublishing("User Service");
            serviceCollection.AddDbContext<UserContext>(
                 options => options.UseSqlServer(connection));



        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            MigrateDatabase(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSharedAppParts("User Service");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        public static void MigrateDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices
                       .GetRequiredService<IServiceScopeFactory>()
                       .CreateScope();
            using var userContext = scope.ServiceProvider.GetService<UserContext>();
            userContext.Database.Migrate();


        }
        private void ConfigureConsul(IServiceCollection services)
        {
            var config = Configuration.GetServiceConfig();

            services.RegisterConsulServices(config);
        }
    }
}
