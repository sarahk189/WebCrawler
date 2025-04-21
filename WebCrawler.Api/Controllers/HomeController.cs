using Microsoft.AspNetCore.Mvc;
using WebCrawler.Core.Interfaces;

namespace WebCrawler.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrawlerController(ICrawlerService crawlerService) : ControllerBase
    {
        private readonly ICrawlerService _crawlerService = crawlerService;

        [HttpGet("pages")]
        public async Task<IActionResult> GetPages([FromQuery] string target)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                return BadRequest(new { error = "Missing or invalid 'target' query parameter." });
            }

            try
            {
                var result = await _crawlerService.CrawlAsync(target);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
