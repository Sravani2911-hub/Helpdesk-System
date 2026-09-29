using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Interfaces
{
    public interface IPriorityService
    {
        Task<IEnumerable<Priority>> GetAllPrioritiesAsync();

        Task<Priority?> GetPriorityByIdAsync(int id);

        Task AddPriorityAsync(Priority priority);

        Task UpdatePriorityAsync(Priority priority);

        Task DeletePriorityAsync(int id);

        Task<IEnumerable<Priority>> SearchPrioritiesAsync(string searchText);
    }
}