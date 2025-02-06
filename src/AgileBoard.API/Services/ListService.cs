using AgileBoard.API.Data;
using AgileBoard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AgileBoard.API.Services
{
    public class ListService : IListService
    {
        private readonly AgileBoardContext _context;

        public ListService(AgileBoardContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<List>> GetAllListsAsync()
        {
            return await _context.Lists
                .Include(l => l.Board)
                .Include(l => l.Cards)
                .OrderBy(l => l.Position)
                .ToListAsync();
        }

        public async Task<List> GetListByIdAsync(int id)
        {
            var list = await _context.Lists
                .Include(l => l.Board)
                .Include(l => l.Cards)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (list == null)
            {
                throw new KeyNotFoundException($"Lista com ID {id} não encontrada.");
            }

            return list;
        }

        public async Task<IEnumerable<List>> GetListsByBoardAsync(int boardId)
        {
            return await _context.Lists
                .Where(l => l.BoardId == boardId)
                .Include(l => l.Cards)
                .OrderBy(l => l.Position)
                .ToListAsync();
        }

        public async Task<List> CreateListAsync(List list)
        {
            if (list == null)
            {
                throw new ArgumentException("Lista não pode ser nula.");
            }

            var maxPosition = await _context.Lists
                .Where(l => l.BoardId == list.BoardId)
                .MaxAsync(l => (int?)l.Position) ?? 0;

            list.Position = maxPosition + 1;
            _context.Lists.Add(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task UpdateListAsync(List list)
        {
            if (list == null)
            {
                throw new ArgumentException("Lista não pode ser nula.");
            }

            var existingList = await _context.Lists.FindAsync(list.Id);
            if (existingList == null)
            {
                throw new KeyNotFoundException($"Lista com ID {list.Id} não encontrada.");
            }

            list.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingList).CurrentValues.SetValues(list);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateListPositionAsync(int id, int newPosition)
        {
            var list = await _context.Lists.FindAsync(id);
            if (list == null)
            {
                throw new KeyNotFoundException($"Lista com ID {id} não encontrada.");
            }

            list.Position = newPosition;
            list.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteListAsync(int id)
        {
            var list = await _context.Lists.FindAsync(id);
            if (list == null)
            {
                throw new KeyNotFoundException($"Lista com ID {id} não encontrada.");
            }

            _context.Lists.Remove(list);
            await _context.SaveChangesAsync();
        }
    }
}