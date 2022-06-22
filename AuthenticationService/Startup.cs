using AuthenticationService.DAL;
using AuthenticationService.Messaging;
using AuthenticationService.Models;
using AuthenticationService.Services;
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

namespace AuthenticationService
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
            var connection = "Server=aukiodb;Database=authdb;User=sa;Password=Your_password123;";

            serviceCollection.AddControllers();


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

            ConfigureConsul(serviceCollection);

            serviceCollection.AddDbContext<UserContext>(
                 options => options.UseSqlServer(connection));


            serviceCollection.AddAuthorization();

            serviceCollection.AddCors();

            serviceCollection.AddSingleton<IJwtAuthenticationManager, JwtAuthenticationManager>();

            serviceCollection.AddSingleton<IHashingService, HashingService>();

            serviceCollection.AddScoped<IAuthDAL, AuthDAL>();

            serviceCollection.AddScoped<IAuthService, AuthService>();

            serviceCollection.AddSharedServices("Authentication Service");

            serviceCollection.AddMessagePublishing("Authentication Service", builder =>
            {
                builder.WithHandler<UserRegisteredMessage>("UserRegistered");
                builder.WithHandler<UserUpdatedMessage>("UserUpdated");
            });

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

            app.UseSharedAppParts("Authentication Service");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
        private void ConfigureConsul(IServiceCollection serviceCollection)
        {
            var config = Configuration.GetServiceConfig();

            serviceCollection.RegisterConsulServices(config);
        }

        private static void MigrateDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices
                        .GetRequiredService<IServiceScopeFactory>()
                        .CreateScope();
            using var userContext = scope.ServiceProvider.GetService<UserContext>();
            userContext.Database.Migrate();
        }
    }
}
