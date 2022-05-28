using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Shared.Messaging
{
    internal class RabbitMqConnection : IDisposable
    {
        private IConnection _connection;
        public IModel CreateChannel()
        {
            var connection = GetConnection();
            return connection.CreateModel();
        }

        private IConnection GetConnection()
        {
            if (_connection == null)
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri("amqp://guest:guest@rabbit"),
                    AutomaticRecoveryEnabled = true 
                };
                _connection = factory.CreateConnection();
            }

            return _connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
