using Microsoft.AspNetCore.Mvc;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;
namespace SportsBetting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetOffersController : ControllerBase
    {
        private readonly IBetOfferRepository _BetOfferRepository;

        private readonly ITeamRepository _teamRepository;
        public BetOffersController(IBetOfferRepository BetOfferRepository, ITeamRepository teamRepository)
        {
            _BetOfferRepository = BetOfferRepository;
          
        }

        // GET: api/BetOffers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BetOffer>>> GetBetOffers()
        {
            return await _BetOfferRepository.Get();
        }

        // GET: api/BetOffers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BetOffer>> GetBetOffer(int id)
        {
            BetOffer? BetOffer = await _BetOfferRepository.GetById(id);
            if (BetOffer == null)
            {
                return NotFound();
            }

            return BetOffer;

        }

        // POST: api/BetOffers
        [HttpPost]
        public async Task<ActionResult<BetOffer>> PostBetOffer(BetOffer BetOffer)
        {
            await _BetOfferRepository.CreateAsync(BetOffer);
            await _BetOfferRepository.SaveChangesAsync();

            return CreatedAtAction("GetBetOffer", new { id = BetOffer.Id }, BetOffer);
        }

        // PUT: api/BetOffers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBetOffer(int id, BetOffer updatedBetOffer)
        {
            if (id != updatedBetOffer.Id)
            {
                return BadRequest();
            }

            var existingBetOffer = await _BetOfferRepository.GetById(id);
            if (existingBetOffer == null)
            {
                return NotFound();
            }

            existingBetOffer.TypeId= updatedBetOffer.TypeId;
            existingBetOffer.Odd = updatedBetOffer.Odd;

            await _BetOfferRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/BetOffers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBetOffer(int id)
        {
            var BetOffer = await _BetOfferRepository.GetById(id);
            if (BetOffer == null)
            {
                return NotFound();
            }

            await _BetOfferRepository.RemoveAsync(BetOffer);
            await _BetOfferRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
