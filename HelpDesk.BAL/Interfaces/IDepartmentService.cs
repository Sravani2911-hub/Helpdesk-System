using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();

        Task<Department?> GetDepartmentByIdAsync(int id);

        Task AddDepartmentAsync(Department department);

        Task UpdateDepartmentAsync(Department department);

        Task DeleteDepartmentAsync(int id);

        Task<IEnumerable<Department>> SearchDepartmentsAsync(string searchText);
    }
}