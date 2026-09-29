using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Interfaces
{
    public interface ITicketHistoryService
    {
        Task AddHistoryAsync(TicketHistory history);

        Task<IEnumerable<TicketHistory>> GetHistoryByTicketIdAsync(int ticketId);
    }
}