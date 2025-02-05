using AgileBoard.API.Data;
using AgileBoard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AgileBoard.API.Services
{
    public class BoardService : IBoardService
    {
        private readonly AgileBoardContext _context;

        public BoardService(AgileBoardContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Board>> GetAllBoardsAsync()
        {
            return await _context.Boards.ToListAsync();
        }

        public async Task<Board> GetBoardByIdAsync(int id)
        {
            return await _context.Boards.FindAsync(id);
        }

        public async Task<Board> CreateBoardAsync(Board board)
        {
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task UpdateBoardAsync(Board board)
        {
            _context.Entry(board).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBoardAsync(int id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board != null)
            {
                _context.Boards.Remove(board);
                await _context.SaveChangesAsync();
            }
        }
    }
}