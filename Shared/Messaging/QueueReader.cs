using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Messaging
{

    internal class QueueReader : IHostedService, IDisposable
    {
        private readonly Repo _repo;
        private readonly ILogger<QueueReader> _logger;
        private IModel _channel;
        private readonly RabbitConfig _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitConnection _connection;
        private readonly Queue _queue;


        public QueueReader(
            ILogger<QueueReader> logger,
            RabbitConfig config,
            IServiceProvider serviceProvider,
            RabbitConnection connection,
            Queue queue,
            Repo repo

        )
        {
            _repo = repo;
            _logger = logger;
            _config = config;
            _serviceProvider = serviceProvider;
            _connection = connection;
            _queue = queue;

        }

        public Task StartAsync(CancellationToken token)
        {
            _config.Configure();

            _channel = _connection.CreateRabbitChannel();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (evt, evt2) =>
            {
                if (HandleMessage(evt2))
                {
                    _channel.BasicAck(evt2.DeliveryTag, false);
                }
                else
                {
                    _channel.BasicReject(evt2.DeliveryTag, true);
                }
            };

            _channel.BasicConsume(_queue.Name, false, consumer);

            return Task.CompletedTask;
        }

        private bool HandleMessage(BasicDeliverEventArgs message)
        {
            if (!message.BasicProperties.Headers.TryGetValue("MessageType", out var objValue) || !(objValue is byte[] valueAsBytes))
            {
                _logger.LogCritical("Unknown message: {Message} discarded in queue {Queue}", Encoding.UTF8.GetString(message.Body.ToArray()), _queue.Name);
                return true;
            }

            var messageType = Encoding.UTF8.GetString(valueAsBytes);
            if (!_repo.TryGettingHandler(messageType, out var implementingHandler))
            {

                _logger.LogInformation("No handler for skipped message of {MessageType}", messageType);
                return true;
            }

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetService(implementingHandler) as IHandler;
                    handler.HandleMessage(messageType, message.Body.ToArray()).GetAwaiter().GetResult();
                }
                _logger.LogInformation("Succesfully handled {MessageType}.", messageType);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unknown exception for {MessageType}.", messageType);
                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Dispose();
            _channel = null;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
