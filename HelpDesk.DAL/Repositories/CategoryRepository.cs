using HelpDesk.DAL.Context;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly HelpDeskDbContext _context;

        public CategoryRepository(HelpDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() //SELECT * FROM Categories
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id) //Select* From Categories Where CategoryId = ...
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category category) //Inserting
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category) //Update
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) //Delete
        {
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> SearchAsync(string searchText) //Search (Select * from Categories Where CategoryName Like '%text%')
        {
            return await _context.Categories
                .Where(c => c.CategoryName.Contains(searchText))
                .ToListAsync();
        }
    }
}