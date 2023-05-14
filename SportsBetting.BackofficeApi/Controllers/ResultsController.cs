using Microsoft.AspNetCore.Mvc;
using SportsBetting.Data.Models;
using SportsBetting.Services.Interfaces;

namespace SportsBetting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultsController(IResultService resultService)
        {
            _resultService = resultService;
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

            return CreatedAtAction("GetResult", new { id = result.Id }, result);
        }
    }
}