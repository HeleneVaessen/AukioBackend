using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace AuthenticationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Try to connect to RabbitMQ");
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    Console.WriteLine("In try");
                    var host = hostBuilder(args).Build();
                    Console.WriteLine("After createhostbuilder");
                    host.Run();
                    Console.WriteLine("After run");
                    break;
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException)
                {
                    Console.WriteLine("Failed attempt " + i + "/5");
                    System.Threading.Thread.Sleep(5000);

                    if (i == 5)
                    {
                        Console.WriteLine("RabbitMQ is offline");
                        break;
                    }
                }
            }
        }
        public static IHostBuilder hostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
