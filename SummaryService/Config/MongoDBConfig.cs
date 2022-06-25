using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryService.Config
{
    public class MongoDBConfig
    {
        public string Database { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
                {
                    Console.WriteLine("No username or password");
                    return $@"mongodb://{Host}:{Port}";
                }
                string s = $@"mongodb://{User}:{Password}@{Host}:{Port}/{Database}?connect=replicaSet";
                Console.WriteLine(s);
                return $@"mongodb://{User}:{Password}@{Host}:{Port}";
            }
        }
    }
}
