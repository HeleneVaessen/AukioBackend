using MongoDB.Driver;
using SummaryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryService.DAL
{
    public class SummaryRepository : ISummaryRepository
    {
        private readonly ISummaryContext _context;

         public SummaryRepository(ISummaryContext context)
        {
            _context = context;
        }

        public async Task Save(Summary summary)
        {
            await _context.Summaries.InsertOneAsync(summary);
        }

        public List<Summary> GetSummaries()
        {
            return _context.Summaries.AsQueryable().ToList();
        }
    }
}
