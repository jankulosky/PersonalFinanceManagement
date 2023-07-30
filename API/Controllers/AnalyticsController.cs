using API.DTOs;
using API.Helpers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AnalyticsController : BaseApiController
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("spending-analytics")]
        public async Task<ActionResult<IEnumerable<AnalyticsListDto>>> GetSpendingAnalytics([FromQuery] AnalyticsParams analyticsParams)
        {
            try
            {
                var result = await _analyticsService.GetTransactionAnalyticsAsync(analyticsParams);

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
