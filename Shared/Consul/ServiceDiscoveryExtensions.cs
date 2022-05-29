using System;
using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shared.Consul
{
    public static class ServiceDiscoveryExtensions
    {
        public static void RegisterConsulServices(this IServiceCollection services, ServiceConfig serviceConfig)
        {
            if (serviceConfig.ServiceName == null)
            {
                throw new ArgumentNullException(nameof(serviceConfig));
            }

            var consulClient = CreateConsulClient(serviceConfig);

            services.AddSingleton(serviceConfig);
            services.AddSingleton<IConsulClient, ConsulClient>(p => consulClient);
            services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
        }

        private static ConsulClient CreateConsulClient(ServiceConfig serviceConfig)
        {
            return new ConsulClient(config =>
            {
                config.Address = serviceConfig.ServiceDiscoveryAddress;
            });
        }
    }
}
