using HelpDesk.DAL.Context;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.DAL.Repositories
{
    public class TicketCommentRepository : ITicketCommentRepository
    {
        private readonly HelpDeskDbContext _context;

        public TicketCommentRepository(HelpDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketComment>> GetCommentsByTicketIdAsync(int ticketId)
        {
            return await _context.TicketComments
                .Include(c => c.User)
                .Where(c => c.TicketId == ticketId)
                .OrderBy(c => c.CommentDate)
                .ToListAsync();
        }

        public async Task AddCommentAsync(TicketComment comment)
        {
            await _context.TicketComments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
    }
}