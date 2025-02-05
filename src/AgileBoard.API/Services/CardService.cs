using AgileBoard.API.Data;
using AgileBoard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AgileBoard.API.Services
{
    public class CardService : ICardService
    {
        private readonly AgileBoardContext _context;

        public CardService(AgileBoardContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            return await _context.Cards
                .Include(c => c.List)
                .Include(c => c.AssignedUser)
                .OrderBy(c => c.Position)
                .ToListAsync();
        }

        public async Task<Card> GetCardByIdAsync(int id)
        {
            var card = await _context.Cards
                .Include(c => c.List)
                .Include(c => c.AssignedUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (card == null)
            {
                throw new KeyNotFoundException($"Card com ID {id} não encontrado.");
            }

            return card;
        }

        public async Task<IEnumerable<Card>> GetCardsByListAsync(int listId)
        {
            return await _context.Cards
                .Where(c => c.ListId == listId)
                .Include(c => c.AssignedUser)
                .OrderBy(c => c.Position)
                .ToListAsync();
        }

        public async Task<Card> CreateCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentException("Card não pode ser nulo.");
            }

            var maxPosition = await _context.Cards
                .Where(c => c.ListId == card.ListId)
                .MaxAsync(c => (int?)c.Position) ?? 0;

            card.Position = maxPosition + 1;
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task UpdateCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentException("Card não pode ser nulo.");
            }

            var existingCard = await _context.Cards.FindAsync(card.Id);
            if (existingCard == null)
            {
                throw new KeyNotFoundException($"Card com ID {card.Id} não encontrado.");
            }

            card.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingCard).CurrentValues.SetValues(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCardPositionAsync(int id, int newPosition)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                throw new KeyNotFoundException($"Card com ID {id} não encontrado.");
            }

            card.Position = newPosition;
            card.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCardAsync(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                throw new KeyNotFoundException($"Card com ID {id} não encontrado.");
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
        }
    }
}