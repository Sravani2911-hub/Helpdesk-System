using HelpDesk.DAL.Models;

namespace HelpDesk.DAL.Interfaces
{
    public interface ITicketHistoryRepository
    {
        Task AddHistoryAsync(TicketHistory history);

        Task<IEnumerable<TicketHistory>> GetHistoryByTicketIdAsync(int ticketId);
    }
}