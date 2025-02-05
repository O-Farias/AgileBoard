using AgileBoard.API.Models;

namespace AgileBoard.API.Services
{
    public interface IBoardService
    {
        Task<IEnumerable<Board>> GetAllBoardsAsync();
        Task<Board> GetBoardByIdAsync(int id);
        Task<Board> CreateBoardAsync(Board board);
        Task UpdateBoardAsync(Board board);
        Task DeleteBoardAsync(int id);
    }
}