using System;

namespace Shared.Consul
{
    public class Config
    {
        public Uri DiscoveryAddress { get; set; }
        public Uri Address { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
