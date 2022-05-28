using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Messaging
{
    internal class MessageHandlerRepo
    {
        private readonly IReadOnlyDictionary<string, Type> _messageHandlers;

        internal MessageHandlerRepo(IReadOnlyDictionary<string, Type> messageHandlers)
        {
            _messageHandlers = messageHandlers;
        }

        public bool TryGetHandlerForMessageType(string messageType, out Type type)
        {
            return _messageHandlers.TryGetValue(messageType, out type);
        }
    }
}
