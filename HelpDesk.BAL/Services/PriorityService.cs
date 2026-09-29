using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Services
{
    public class PriorityService : IPriorityService
    {
        private readonly IPriorityRepository _priorityRepository;

        public PriorityService(IPriorityRepository priorityRepository)
        {
            _priorityRepository = priorityRepository;
        }

        public async Task<IEnumerable<Priority>> GetAllPrioritiesAsync()
        {
            return await _priorityRepository.GetAllAsync();
        }

        public async Task<Priority?> GetPriorityByIdAsync(int id)
        {
            return await _priorityRepository.GetByIdAsync(id);
        }

        public async Task AddPriorityAsync(Priority priority)
        {
            await _priorityRepository.AddAsync(priority);
        }

        public async Task UpdatePriorityAsync(Priority priority)
        {
            await _priorityRepository.UpdateAsync(priority);
        }

        public async Task DeletePriorityAsync(int id)
        {
            await _priorityRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Priority>> SearchPrioritiesAsync(string searchText)
        {
            return await _priorityRepository.SearchAsync(searchText);
        }
    }
}