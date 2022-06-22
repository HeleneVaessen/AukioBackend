using RabbitMQ.Client;
using System;

namespace Shared.Messaging
{
    internal class RabbitConnection : IDisposable
    {
        private IConnection _connection;
        public IModel CreateRabbitChannel()
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
