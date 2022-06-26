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
using SummaryService.Config;
using SummaryService.DAL;
using SummaryService.Services;
using System.Text;

namespace SummaryService
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

            var config = new ServerConfig();
            Configuration.Bind(config);

            var summaryContext = new SummaryContext(config.MongoDB);

            var repo = new SummaryRepository(summaryContext);

            serviceCollection.AddSingleton<ISummaryRepository>(repo);
  
            serviceCollection.AddCors();

            serviceCollection.AddSharedServices("Summary Service");

            serviceCollection.AddScoped<ISummaryService, Services.SummaryService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

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

            app.UseSharedAppParts("Summary Service");

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
    }
}
