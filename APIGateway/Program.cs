using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;


namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            hostBuilder(args).Build().Run();
        }

        public static IHostBuilder hostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    webBuilder.ConfigureAppConfiguration(config =>
                    config.AddJsonFile($"ocelot{environment}.json"));
                })
            .ConfigureLogging(logging => logging.AddConsole());
    }
}
