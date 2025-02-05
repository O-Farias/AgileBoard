using AgileBoard.API.Models;

namespace AgileBoard.API.Services
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> GetAllCardsAsync();
        Task<Card> GetCardByIdAsync(int id);
        Task<IEnumerable<Card>> GetCardsByListAsync(int listId);
        Task<Card> CreateCardAsync(Card card);
        Task UpdateCardAsync(Card card);
        Task UpdateCardPositionAsync(int id, int position);
        Task DeleteCardAsync(int id);
    }
}