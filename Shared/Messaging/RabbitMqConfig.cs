using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messaging
{
    internal class RabbitMqConfig
    {
        private readonly RabbitMqConnection _connection;
        private bool configured = false;

        public RabbitMqConfig(RabbitMqConnection connection)
        {
            _connection = connection;
        }

        public void ConfigureRabbit()
        {
            if (!configured)
            {
                var channel = _connection.CreateChannel();

                channel.ExchangeDeclare("aukio", "fanout");

                channel.QueueDeclare("Authentication Service", false, false);
                channel.QueueDeclare("User Service", false, false);

                channel.QueueBind("Authentication Service", "aukio", "UserRegistered");

                configured = true;
            }
        }
    }
}
