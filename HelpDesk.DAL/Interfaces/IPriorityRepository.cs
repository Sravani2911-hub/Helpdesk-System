using HelpDesk.DAL.Models;

namespace HelpDesk.DAL.Interfaces
{
    public interface IPriorityRepository
    {
        Task<IEnumerable<Priority>> GetAllAsync();

        Task<Priority?> GetByIdAsync(int id);

        Task AddAsync(Priority priority);

        Task UpdateAsync(Priority priority);

        Task DeleteAsync(int id);

        Task<IEnumerable<Priority>> SearchAsync(string searchText);
    }
}