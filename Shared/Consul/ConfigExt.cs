using Microsoft.Extensions.Configuration;
using System;

namespace Shared.Consul
{
    public static class ConfigExt
    {
        public static Config GetConfig(this IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var serviceConfig = new Config
            {
                DiscoveryAddress = config.GetValue<Uri>("ServiceConfig:serviceDiscoveryAddress"),
                Address = config.GetValue<Uri>("ServiceConfig:serviceAddress"),
                Name = config.GetValue<string>("ServiceConfig:serviceName"),
                Id = config.GetValue<string>("ServiceConfig:serviceId")
            };

            if (serviceConfig.Name == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return serviceConfig;
        }
    }
}