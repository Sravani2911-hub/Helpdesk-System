using HelpDesk.DAL.Context;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.DAL.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly HelpDeskDbContext _context;

        public TicketRepository(HelpDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets
                .Include(t => t.Department)
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Department)
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.TicketId == id);
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            var existingTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TicketId == ticket.TicketId);

            if (existingTicket == null)
                return;

            existingTicket.Status = ticket.Status;
            existingTicket.Title = ticket.Title;
            existingTicket.Description = ticket.Description;
            existingTicket.DepartmentId = ticket.DepartmentId;
            existingTicket.CategoryId = ticket.CategoryId;
            existingTicket.PriorityId = ticket.PriorityId;
            existingTicket.AssignedToId = ticket.AssignedToId;

            // Preserve original values
            existingTicket.CreatedById = existingTicket.CreatedById;
            existingTicket.CreatedDate = existingTicket.CreatedDate;
            existingTicket.TicketNumber = existingTicket.TicketNumber;
            existingTicket.AttachmentPath = ticket.AttachmentPath;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Ticket>> SearchAsync(string searchText)
        {
            return await _context.Tickets //Ticket is related to multiple tables. So we use
                .Include(t => t.Department)  //This tells Entity Framework Core to fetch the related data as well.
                .Include(t => t.Category)  //Without Include(), you would only get IDs like:DepartmentId = 1,CategoryId = 2,PriorityId = 3
                .Include(t => t.Priority)  //With Include(), you can directly access:ticket.Department.DepartmentName etc
                .Where(t => t.Title.Contains(searchText) ||
                            t.TicketNumber.Contains(searchText))
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAssignedTicketsAsync(string userId)
        {
            return await _context.Tickets
                .Include(t => t.Department)
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.AssignedTo)
                .Where(t => t.AssignedToId == userId)
                .ToListAsync();
        }

        public async Task<int> GetTotalTicketsAsync() //These methods count tickets directly from SQL Server.
        {
            return await _context.Tickets.CountAsync(); //Count all tickets where the Status is Open.
        }

        public async Task<int> GetOpenTicketsAsync()
        {
            return await _context.Tickets.CountAsync(t => t.Status == "Open");
        }

        public async Task<int> GetInProgressTicketsAsync()
        {
            return await _context.Tickets.CountAsync(t => t.Status == "In Progress");
        }

        public async Task<int> GetResolvedTicketsAsync()
        {
            return await _context.Tickets.CountAsync(t => t.Status == "Resolved");
        }

        public async Task<int> GetClosedTicketsAsync()
        {
            return await _context.Tickets.CountAsync(t => t.Status == "Closed");
        }

        public async Task<IEnumerable<Ticket>> GetMyTicketsAsync(string userId)
        {
            return await _context.Tickets
                .Include(t => t.Department)
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.CreatedBy)
                .Where(t => t.CreatedById == userId)
                .ToListAsync();       //It retrieves only the tickets where:CreatedById == Logged-in Employee UserId
        }
        
    }
}