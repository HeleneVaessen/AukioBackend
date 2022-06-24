using MongoDB.Driver;
using SummaryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryService.DAL
{
    public interface ISummaryContext
    {
        IMongoCollection<Summary> Summaries { get; }
    }
}
