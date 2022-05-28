using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Messaging
{
    internal class QueueName
    {
        public string Name { get; }
        public QueueName(string name)
        {
            Name = name;
        }
    }
}
