using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Interfaces
{
    public interface ITicketCommentService
    {
        Task<IEnumerable<TicketComment>> GetCommentsByTicketIdAsync(int ticketId);

        Task AddCommentAsync(TicketComment comment);
    }
}