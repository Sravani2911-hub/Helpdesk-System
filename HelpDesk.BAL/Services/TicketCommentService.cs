using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Services
{
    public class TicketCommentService : ITicketCommentService
    {
        private readonly ITicketCommentRepository _ticketCommentRepository;

        public TicketCommentService(ITicketCommentRepository ticketCommentRepository)
        {
            _ticketCommentRepository = ticketCommentRepository;
        }

        public async Task<IEnumerable<TicketComment>> GetCommentsByTicketIdAsync(int ticketId)
        {
            return await _ticketCommentRepository.GetCommentsByTicketIdAsync(ticketId);
        }

        public async Task AddCommentAsync(TicketComment comment)
        {
            await _ticketCommentRepository.AddCommentAsync(comment);
        }
    }
}