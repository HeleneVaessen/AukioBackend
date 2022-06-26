using SummaryService.DAL;
using SummaryService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SummaryService.Test
{
    public class MockSummaryRepository : ISummaryRepository
    {
        private List<Summary> summaries = new List<Summary>();
        public List<Summary> GetSummaries()
        {
            return summaries;
        }

        public Task Save(Summary summary)
        {
            summaries.Add(summary);

            return Task.CompletedTask;
        }
    }
}
