using HelpDesk.DAL.Context;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.DAL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HelpDeskDbContext _context;

        public DepartmentRepository(HelpDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync() //Returns all departments.(SELECT * FROM Departments;)
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id); //Returns one department.(SELECT * FROM Departments WHERE DepartmentId = 1;)
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department); //INSERT INTO Departments
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)  //UPDATE Departments SET DepartmentName='IT' WHERE DepartmentId=1;
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) //DELETE FROM Departments WHERE DepartmentId=1;
        {
            var department = await _context.Departments.FindAsync(id);

            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Department>> SearchAsync(string searchText)
        {
            return await _context.Departments
                .Where(d => d.DepartmentName.Contains(searchText))
                .ToListAsync();
        }
    }
}