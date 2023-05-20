using Microsoft.AspNetCore.Mvc;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;
using SportsBetting.Services.Services.Interfaces;
namespace SportsBetting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetOffersController : ControllerBase
    {
        private readonly IBetOfferService _betOfferService;

        private readonly ITeamRepository _teamRepository;
        public BetOffersController(IBetOfferService betOfferService)
        {
            _betOfferService = betOfferService;
        }

        // GET: api/BetOffers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BetOffer>>> GetBetOffers()
        {
            var betOffers = await _betOfferService.GetAllAsync();
            return Ok(betOffers);
        }

        // GET: api/BetOffers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BetOffer>> GetBetOffer(int id)
        {
            var betOffer = await _betOfferService.GetByIdAsync(id);
            if (betOffer == null)
            {
                return NotFound();
            }

            return Ok(betOffer);

        }

        // POST: api/BetOffers
        [HttpPost]
        public async Task<ActionResult<BetOffer>> PostBetOffer(BetOffer BetOffer)
        {
            var createdBetOffer = await _betOfferService.CreateAsync(BetOffer);
            return CreatedAtAction(nameof(GetBetOffer), new { id = createdBetOffer.Id }, createdBetOffer);
        }

        // PUT: api/BetOffers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBetOffer(int id, BetOffer updatedBetOffer)
        {
            await _betOfferService.UpdateAsync(id, updatedBetOffer);
            return NoContent();
        }

        // DELETE: api/BetOffers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBetOffer(int id)
        {
            await _betOfferService.DeleteAsync(id);
            return NoContent();
        }
    }
}
