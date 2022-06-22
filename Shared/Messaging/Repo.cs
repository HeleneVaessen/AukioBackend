using System;
using System.Collections.Generic;

namespace Shared.Messaging
{

    internal class Repo
    {
        private readonly IReadOnlyDictionary<string, Type> _handlers;

        internal Repo(IReadOnlyDictionary<string, Type> handlers)
        {
            _handlers = handlers;
        }

        public bool TryGettingHandler(string messageType, out Type type)
        {
            return _handlers.TryGetValue(messageType, out type);
        }
    }
}
