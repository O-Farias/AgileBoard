using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgileBoard.API.Data;
using AgileBoard.API.Models;

namespace AgileBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly AgileBoardContext _context;

        public BoardsController(AgileBoardContext context)
        {
            _context = context;
        }

        // GET: api/Boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Board>>> GetBoards()
        {
            return await _context.Boards.ToListAsync();
        }

        // GET: api/Boards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Board>> GetBoard(int id)
        {
            var board = await _context.Boards.FindAsync(id);

            if (board == null)
            {
                return NotFound();
            }

            return board;
        }

        // POST: api/Boards
        [HttpPost]
        public async Task<ActionResult<Board>> PostBoard(Board board)
        {
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBoard), new { id = board.Id }, board);
        }

        // PUT: api/Boards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoard(int id, Board board)
        {
            if (id != board.Id)
            {
                return BadRequest();
            }

            _context.Entry(board).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Boards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoardExists(int id)
        {
            return _context.Boards.Any(e => e.Id == id);
        }
    }
}