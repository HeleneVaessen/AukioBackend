using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Shared.Messaging
{
    public interface IMessageHandler
    {
        Task HandleMessageAsync(string messageType, byte[] obj);
    }

    public interface IMessageHandler<in TMessage> : IMessageHandler
        where TMessage : class
    {
        Task IMessageHandler.HandleMessageAsync(string messageType, byte[] obj)
        {
            return HandleMessageAsync(messageType, JsonSerializer.Deserialize<TMessage>(obj));
        }

        Task HandleMessageAsync(string messageType, TMessage message);
    }
}
