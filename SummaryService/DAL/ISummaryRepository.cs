using SummaryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryService.DAL
{
    public interface ISummaryRepository
    {
        Task Save(Summary summary);
        List<Summary> GetSummaries();
    }
}
