using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Messaging
{

    public interface IHandler
    {
        Task HandleMessage(string messageType, byte[] obj);
    }


    public interface IHandler<in TMessage> : IHandler
        where TMessage : class
    {
    
        Task IHandler.HandleMessage(string messageType, byte[] obj)
        {
            return HandleMessage(messageType, JsonSerializer.Deserialize<TMessage>(obj));
        }

        Task HandleMessage(string messageType, TMessage message);
    }
}
