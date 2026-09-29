using HelpDesk.DAL.Models;

namespace HelpDesk.DAL.Interfaces
{
    public interface ITicketCommentRepository
    {
        Task<IEnumerable<TicketComment>> GetCommentsByTicketIdAsync(int ticketId);

        Task AddCommentAsync(TicketComment comment);
    }
}