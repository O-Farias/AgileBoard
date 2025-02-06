using Microsoft.AspNetCore.SignalR;
using AgileBoard.API.Models;

namespace AgileBoard.API.Hubs
{
    public class BoardHub : Hub
    {
        public async Task JoinBoard(string boardId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, boardId);
        }

        public async Task LeaveBoard(string boardId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, boardId);
        }

        public async Task NotifyCardMoved(string boardId, int cardId, int listId, int position)
        {
            await Clients.Group(boardId).SendAsync("CardMoved", cardId, listId, position);
        }

        public async Task NotifyCardCreated(string boardId, Card card)
        {
            await Clients.Group(boardId).SendAsync("CardCreated", card);
        }

        public async Task NotifyCardUpdated(string boardId, Card card)
        {
            await Clients.Group(boardId).SendAsync("CardUpdated", card);
        }

        public async Task NotifyCardDeleted(string boardId, int cardId)
        {
            await Clients.Group(boardId).SendAsync("CardDeleted", cardId);
        }
    }
}