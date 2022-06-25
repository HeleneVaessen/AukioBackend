using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SummaryService.Models;
using SummaryService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryService _summaryService;

        public SummaryController(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        [HttpPost("postSummary")]
        public async Task<IActionResult> PostSummary([FromBody] Summary summary)
        {
            if (summary.UserId != 0)
            {
                await _summaryService.PostSummary(summary);
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("getSummaries")]
        public async Task<IActionResult> GetSummaries()
        {
            var summaries = _summaryService.GetSummaries();

            return Ok(summaries);
        }
    }
}
