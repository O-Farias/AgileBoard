using AgileBoard.API.Models;

namespace AgileBoard.API.Services
{
    public interface IListService
    {
        Task<IEnumerable<List>> GetAllListsAsync();
        Task<List> GetListByIdAsync(int id);
        Task<IEnumerable<List>> GetListsByBoardAsync(int boardId);
        Task<List> CreateListAsync(List list);
        Task UpdateListAsync(List list);
        Task UpdateListPositionAsync(int id, int position);
        Task DeleteListAsync(int id);
    }
}