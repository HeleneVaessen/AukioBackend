using SummaryService.DAL;
using SummaryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryService.Services
{
    public class SummaryService : ISummaryService
    {
        private readonly ISummaryRepository _summaryRepository;

        public SummaryService(ISummaryRepository summaryRepository)
        {
            _summaryRepository = summaryRepository;
        }

        public async Task PostSummary(Summary summary)
        {
            summary.SummaryID = Guid.NewGuid();
            await _summaryRepository.Save(summary);
        }
    }
}
