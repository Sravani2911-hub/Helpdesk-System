using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _ticketRepository.GetAllAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            await _ticketRepository.AddAsync(ticket);
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task DeleteTicketAsync(int id)
        {
            await _ticketRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Ticket>> SearchTicketsAsync(string searchText)
        {
            return await _ticketRepository.SearchAsync(searchText);
        }

        public async Task<int> GetTotalTicketsAsync()
        {
            return await _ticketRepository.GetTotalTicketsAsync();
        }

        public async Task<int> GetOpenTicketsAsync()
        {
            return await _ticketRepository.GetOpenTicketsAsync();
        }

        public async Task<int> GetInProgressTicketsAsync()
        {
            return await _ticketRepository.GetInProgressTicketsAsync();
        }

        public async Task<int> GetResolvedTicketsAsync()
        {
            return await _ticketRepository.GetResolvedTicketsAsync();
        }

        public async Task<int> GetClosedTicketsAsync()
        {
            return await _ticketRepository.GetClosedTicketsAsync();
        }
        public async Task<IEnumerable<Ticket>> GetAssignedTicketsAsync(string userId)
        {
            return await _ticketRepository.GetAssignedTicketsAsync(userId);
        }
        public async Task<IEnumerable<Ticket>> GetMyTicketsAsync(string userId)
        {
            return await _ticketRepository.GetMyTicketsAsync(userId);
        }
    }
}