using HelpDesk.DAL.Models;

namespace HelpDesk.DAL.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync();

        Task<Ticket?> GetByIdAsync(int id);

        Task AddAsync(Ticket ticket);

        Task UpdateAsync(Ticket ticket);

        Task DeleteAsync(int id);



        Task<IEnumerable<Ticket>> SearchAsync(string searchText);

        // Dashboard Methods
        Task<int> GetTotalTicketsAsync();

        Task<int> GetOpenTicketsAsync();

        Task<int> GetInProgressTicketsAsync();

        Task<int> GetResolvedTicketsAsync();

        Task<int> GetClosedTicketsAsync();

        Task<IEnumerable<Ticket>> GetAssignedTicketsAsync(string userId);

        Task<IEnumerable<Ticket>> GetMyTicketsAsync(string userId);
    }
}