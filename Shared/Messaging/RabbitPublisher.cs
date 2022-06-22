using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Messaging
{
    internal class RabbitPublisher : IPublisher
    {
        private readonly RabbitConnection _rabbitConnection;

        public RabbitPublisher(RabbitConnection rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

        public Task PublishMessage<T>(string messageType, T value)
        {
            using var channel = _rabbitConnection.CreateRabbitChannel();
            var body = JsonSerializer.SerializeToUtf8Bytes(value);
            var message = channel.CreateBasicProperties();

            message.DeliveryMode = 2;
            message.Headers = new Dictionary<string, object> { ["MessageType"] = messageType };
            message.ContentType = "application/json";
            channel.BasicPublish("aukio", string.Empty, message, body);

            return Task.CompletedTask;
        }
    }
}
