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
            var board = await _context.Boards.FindAsync(id);
            if (board == null)
            {
                throw new KeyNotFoundException($"Board com ID {id} não encontrado.");
            }
            return board;
        }

        public async Task<Board> CreateBoardAsync(Board board)
        {
            if (board == null)
            {
                throw new ArgumentException("Board não pode ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(board.Name))
            {
                throw new ArgumentException("Nome do board é obrigatório.");
            }

            _context.Boards.Add(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task UpdateBoardAsync(Board board)
        {
            if (board == null)
            {
                throw new ArgumentException("Board não pode ser nulo.");
            }

            var existingBoard = await _context.Boards.FindAsync(board.Id);
            if (existingBoard == null)
            {
                throw new KeyNotFoundException($"Board com ID {board.Id} não encontrado.");
            }

            _context.Entry(board).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBoardAsync(int id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board == null)
            {
                throw new KeyNotFoundException($"Board com ID {id} não encontrado.");
            }

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
        }
    }
}