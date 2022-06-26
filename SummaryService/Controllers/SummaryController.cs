using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
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
                var address = $@"https://swearwordfunction.azurewebsites.net/api/swearwordchecker?name= {summary.Title} {summary.Content}";
                var client = new RestClient(address);

                var request = new RestRequest();

                var response = await client.ExecuteGetAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await _summaryService.PostSummary(summary);
                    return Ok();
                }             
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
