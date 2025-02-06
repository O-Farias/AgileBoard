using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AgileBoard.API.Models;
using AgileBoard.API.Services;

namespace AgileBoard.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            return Ok(await _cardService.GetAllCardsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var card = await _cardService.GetCardByIdAsync(id);
            return Ok(card);
        }

        [HttpGet("list/{listId}")]
        public async Task<ActionResult<IEnumerable<Card>>> GetCardsByList(int listId)
        {
            return Ok(await _cardService.GetCardsByListAsync(listId));
        }

        [HttpPost]
        public async Task<ActionResult<Card>> CreateCard(Card card)
        {
            var createdCard = await _cardService.CreateCardAsync(card);
            return CreatedAtAction(nameof(GetCard), new { id = createdCard.Id }, createdCard);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCard(int id, Card card)
        {
            if (id != card.Id)
            {
                return BadRequest();
            }

            await _cardService.UpdateCardAsync(card);
            return NoContent();
        }

        [HttpPut("{id}/position")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] int position)
        {
            await _cardService.UpdateCardPositionAsync(id, position);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            await _cardService.DeleteCardAsync(id);
            return NoContent();
        }
    }
}