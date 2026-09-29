using HelpDesk.DAL.Context;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.DAL.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly HelpDeskDbContext _context;

        public PriorityRepository(HelpDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Priority>> GetAllAsync()
        {
            return await _context.Priorities.ToListAsync();
        }

        public async Task<Priority?> GetByIdAsync(int id)
        {
            return await _context.Priorities.FindAsync(id);
        }

        public async Task AddAsync(Priority priority)
        {
            await _context.Priorities.AddAsync(priority);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Priority priority)
        {
            _context.Priorities.Update(priority);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);

            if (priority != null)
            {
                _context.Priorities.Remove(priority);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Priority>> SearchAsync(string searchText)
        {
            return await _context.Priorities
                .Where(p => p.PriorityName.Contains(searchText))
                .ToListAsync();
        }
    }
}