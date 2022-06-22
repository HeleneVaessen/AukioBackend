using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Shared.Consul
{
    public static class DiscoveryExt
    {
        public static void RegisterConsulServices(this IServiceCollection serviceCollection, Config config)
        {
            if (config.Name == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var client = CreateClient(config);

            serviceCollection.AddSingleton(config);
            serviceCollection.AddSingleton<IConsulClient, ConsulClient>(p => client);
            serviceCollection.AddSingleton<IHostedService, Discovery>();
        }

        private static ConsulClient CreateClient(Config serviceConfig)
        {
            return new ConsulClient(config =>
            {
                config.Address = serviceConfig.DiscoveryAddress;
            });
        }
    }
}
