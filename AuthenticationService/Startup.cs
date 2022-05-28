using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.DAL;
using AuthenticationService.Messaging;
using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.Consul;

namespace AuthenticationService
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var key = "pk";

            ConfigureConsul(services);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false

                };
            }
            );

            services.AddAuthorization();

            services.AddCors();

            var connection = "Server=authdb;Database=aukio;User=sa;Password=Your_password123;";

            services.AddDbContext<UserContext>(
                 options => options.UseSqlServer(connection));

            services.AddSingleton<IJwtAuthenticationManager, JwtAuthenticationManager>();

            services.AddSingleton<IHashingService, HashingService>();

            services.AddScoped<IUserDAL, UserDAL>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddSharedServices("Authentication Service");

            services.AddMessagePublishing("Authentication Service", builder => {
                builder.WithHandler<UserRegisteredMessage>("UserRegistered");
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                        .GetRequiredService<IServiceScopeFactory>()
                        .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<UserContext>();
            context.Database.Migrate();
        }
        private void ConfigureConsul(IServiceCollection services)
        {
            var serviceConfig = Configuration.GetServiceConfig();

            services.RegisterConsulServices(serviceConfig);
        }
    }
}
