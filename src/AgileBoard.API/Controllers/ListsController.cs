using Microsoft.AspNetCore.Mvc;
using AgileBoard.API.Models;
using AgileBoard.API.Services;

namespace AgileBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly IListService _listService;

        public ListsController(IListService listService)
        {
            _listService = listService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<List>>> GetLists()
        {
            return Ok(await _listService.GetAllListsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List>> GetList(int id)
        {
            var list = await _listService.GetListByIdAsync(id);
            return Ok(list);
        }

        [HttpGet("board/{boardId}")]
        public async Task<ActionResult<IEnumerable<List>>> GetListsByBoard(int boardId)
        {
            return Ok(await _listService.GetListsByBoardAsync(boardId));
        }

        [HttpPost]
        public async Task<ActionResult<List>> CreateList(List list)
        {
            var createdList = await _listService.CreateListAsync(list);
            return CreatedAtAction(nameof(GetList), new { id = createdList.Id }, createdList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateList(int id, List list)
        {
            if (id != list.Id)
            {
                return BadRequest();
            }

            await _listService.UpdateListAsync(list);
            return NoContent();
        }

        [HttpPut("{id}/position")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] int position)
        {
            await _listService.UpdateListPositionAsync(id, position);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            await _listService.DeleteListAsync(id);
            return NoContent();
        }
    }
}