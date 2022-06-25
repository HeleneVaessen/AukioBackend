using SummaryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryService.Services
{
    public interface ISummaryService
    {
        Task PostSummary(Summary summary);

        List<Summary> GetSummaries();
    }
}
