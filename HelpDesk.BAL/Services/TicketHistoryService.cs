using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Services
{
    public class TicketHistoryService : ITicketHistoryService
    {
        private readonly ITicketHistoryRepository _historyRepository;

        public TicketHistoryService(ITicketHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task AddHistoryAsync(TicketHistory history)
        {
            await _historyRepository.AddHistoryAsync(history);
        }

        public async Task<IEnumerable<TicketHistory>> GetHistoryByTicketIdAsync(int ticketId)
        {
            return await _historyRepository.GetHistoryByTicketIdAsync(ticketId);
        }
    }
}