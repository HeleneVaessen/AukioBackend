using RabbitMQ.Client;

namespace Shared.Messaging
{
    internal class RabbitConfig
    {
        private readonly RabbitConnection _rabbitConnection;
        private bool configured = false;

        public RabbitConfig(RabbitConnection rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

        public void Configure()
        {
            if (!configured)
            {
                var channel = _rabbitConnection.CreateRabbitChannel();

                channel.ExchangeDeclare("aukio", "fanout");

                channel.QueueDeclare("Authentication Service", false, false);
                channel.QueueDeclare("User Service", false, false);

                channel.QueueBind("Authentication Service", "aukio", "UserRegistered");
                channel.QueueBind("Authentication Service", "aukio", "UserUpdated");

                configured = true;
            }
        }
    }
}
