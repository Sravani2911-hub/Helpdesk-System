using HelpDesk.DAL.Models;

namespace HelpDesk.DAL.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync(); //Get all departments

        Task<Department?> GetByIdAsync(int id); //Get one department

        Task AddAsync(Department department); //Add a department

        Task UpdateAsync(Department department); //Update a department

        Task DeleteAsync(int id); //Delete a department

        Task<IEnumerable<Department>> SearchAsync(string searchText);
    }
}