using Shared.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Test
{
    class MockMssgPublisher :IMessagePublisher
    {
        public Task PublishMessageAsync<T>(string messageType, T value)
        {
            return Task.CompletedTask;
        }
    }
}
