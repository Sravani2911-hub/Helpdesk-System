using HelpDesk.DAL.Context;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.DAL.Repositories
{
    public class TicketHistoryRepository : ITicketHistoryRepository
    {
        private readonly HelpDeskDbContext _context;

        public TicketHistoryRepository(HelpDeskDbContext context)
        {
            _context = context;
        }

        public async Task AddHistoryAsync(TicketHistory history)
        {
            _context.TicketHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TicketHistory>> GetHistoryByTicketIdAsync(int ticketId)
        {
            return await _context.TicketHistories
                .Include(h => h.User)
                .Where(h => h.TicketId == ticketId)
                .OrderByDescending(h => h.ActionDate)
                .ToListAsync();
        }
    }
}