using Consul;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Consul
{
    public class Discovery : IHostedService
    {
        private readonly IConsulClient _client;
        private readonly Config _config;
        private string id;

        public Discovery(IConsulClient client, Config config)
        {
            _client = client;
            _config = config;
            if (config.Name == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
        }

        public async Task StartAsync(CancellationToken token)
        {
            id = $"{_config.Name}-{_config.Id}";

            var registration = new AgentServiceRegistration
            {
                ID = id,
                Name = _config.Name,
                Port = _config.Address.Port,
                Address = _config.Address.Host
            };

            await _client.Agent.ServiceDeregister(registration.ID, token);
            await _client.Agent.ServiceRegister(registration, token);
        }

        public async Task StopAsync(CancellationToken token)
        {
            await _client.Agent.ServiceDeregister(id, token);
        }
    }
}