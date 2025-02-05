using Microsoft.AspNetCore.Mvc;
using AgileBoard.API.Models;
using AgileBoard.API.Services;

namespace AgileBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardsController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        // GET: api/Boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Board>>> GetBoards()
        {
            return Ok(await _boardService.GetAllBoardsAsync());
        }

        // GET: api/Boards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Board>> GetBoard(int id)
        {
            var board = await _boardService.GetBoardByIdAsync(id);

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
            var createdBoard = await _boardService.CreateBoardAsync(board);
            return CreatedAtAction(nameof(GetBoard), new { id = createdBoard.Id }, createdBoard);
        }

        // PUT: api/Boards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoard(int id, Board board)
        {
            if (id != board.Id)
            {
                return BadRequest();
            }

            try
            {
                await _boardService.UpdateBoardAsync(board);
            }
            catch (Exception)
            {
                if (!await BoardExists(id))
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
            var board = await _boardService.GetBoardByIdAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            await _boardService.DeleteBoardAsync(id);

            return NoContent();
        }

        private async Task<bool> BoardExists(int id)
        {
            return await _boardService.GetBoardByIdAsync(id) != null;
        }
    }
}