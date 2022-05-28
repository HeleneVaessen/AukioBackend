using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Messaging
{
    public interface IMessagePublisher
    {
Task PublishMessageAsync<T>(string messageType, T value);
    }
}
