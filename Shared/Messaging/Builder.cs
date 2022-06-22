using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shared.Messaging
{
    public class Builder
    {

        private readonly IServiceCollection _serviceCollection;


        private readonly Dictionary<string, Type> _handlers = new Dictionary<string, Type>();

        internal IReadOnlyDictionary<string, Type> Handlers => new ReadOnlyDictionary<string, Type>(_handlers);

        internal Builder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public Builder buildHandler<THandler>(string messageType)
            where THandler : IHandler
        {
            var type = typeof(THandler);
            _serviceCollection.AddScoped(type);
            _handlers.Add(messageType, type);
            return this;
        }
    }
}
