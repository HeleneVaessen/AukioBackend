using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryService.Models
{
    public class Summary
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Guid SummaryID { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
