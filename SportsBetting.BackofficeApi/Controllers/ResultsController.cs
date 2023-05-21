using Microsoft.AspNetCore.Mvc;
using SportsBetting.Data.Models;
using SportsBetting.Services.Services.Interfaces;

namespace SportsBetting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IResultService _resultService;

        private readonly ILogger<ResultsController> _logger;

        public ResultsController(IResultService resultService, ILogger<ResultsController> logger)
        {
            _resultService = resultService;
            _logger = logger;
        }

        // POST: api/Results
        [HttpPost]
        public async Task<ActionResult<Result>> PostResult(Result result)
        {
            try
            {
                await _resultService.CreateResultAsync(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _logger.LogInformation($"Manual update for result: {System.Text.Json.JsonSerializer.Serialize(result)}");

            return CreatedAtAction("GetResult", new { id = result.Id }, result);
        }
    }
}