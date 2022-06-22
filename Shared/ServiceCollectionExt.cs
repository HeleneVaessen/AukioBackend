using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Shared.Messaging;
using System;

namespace Shared
{
    public static class ServiceCollectionExt

    {
        public static void AddServices(this IServiceCollection serviceCollection, string api)
        {
            serviceCollection.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = api, Version = "v1" }));

            serviceCollection.AddLogging(logging =>
            {
                logging
                    .AddSeq("http://localhost:7201")
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Debug);
            });
        }

        public static void PublishMessages(this IServiceCollection serviceCollection, string queue, Action<Builder> builderFn = null)
        {
            var builder = new Builder(serviceCollection);
            var queueService = new Queue(queue);
            var rabbitConnection = new RabbitConnection();

            serviceCollection.AddHostedService<QueueReader>();
            serviceCollection.AddSingleton(new Repo(builder.Handlers));
            serviceCollection.AddSingleton<RabbitConfig>();
            serviceCollection.AddScoped<IPublisher, RabbitPublisher>();
            serviceCollection.AddSingleton(queueService);
            serviceCollection.AddSingleton(rabbitConnection);

            builderFn?.Invoke(builder);
        }
    }
}
