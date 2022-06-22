using System.Threading.Tasks;

namespace Shared.Messaging
{
    public interface IPublisher
    {
        Task PublishMessage<T>(string messageType, T value);
    }
}
