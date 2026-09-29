using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository; //The Service doesn't communicate with SQL Server directly.

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            // Business Logic can be added here

            await _departmentRepository.AddAsync(department);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            await _departmentRepository.UpdateAsync(department);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            await _departmentRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Department>> SearchDepartmentsAsync(string searchText)
        {
            return await _departmentRepository.SearchAsync(searchText);
        }
    }
}