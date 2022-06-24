using MongoDB.Driver;
using SummaryService.Config;
using SummaryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryService.DAL
{
    public class SummaryContext : ISummaryContext
    {
        private readonly IMongoDatabase _db;

        public SummaryContext(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }
        public IMongoCollection<Summary> Summaries => _db.GetCollection<Summary>("Summaries");

    }
}
