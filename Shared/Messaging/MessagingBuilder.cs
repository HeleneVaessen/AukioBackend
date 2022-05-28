using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Shared.Messaging
{
    public class MessagingBuilder
    {
 
        private readonly IServiceCollection _services;

        private readonly Dictionary<string, Type> _messageHandlers = new Dictionary<string, Type>();


        internal IReadOnlyDictionary<string, Type> MessageHandlers => new ReadOnlyDictionary<string, Type>(_messageHandlers);

        internal MessagingBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public MessagingBuilder WithHandler<THandler>(string messageType)
            where THandler : IMessageHandler
        {
            var type = typeof(THandler);
            _services.AddScoped(type);
            _messageHandlers.Add(messageType, type);
            return this;
        }
    }
}
