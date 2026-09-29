using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();

        Task<Ticket?> GetTicketByIdAsync(int id);

        Task AddTicketAsync(Ticket ticket);

        Task UpdateTicketAsync(Ticket ticket);

        Task DeleteTicketAsync(int id);


        Task<IEnumerable<Ticket>> SearchTicketsAsync(string searchText);

        Task<int> GetTotalTicketsAsync();

        Task<int> GetOpenTicketsAsync();

        Task<int> GetInProgressTicketsAsync();

        Task<int> GetResolvedTicketsAsync();

        Task<int> GetClosedTicketsAsync();

        Task<IEnumerable<Ticket>> GetAssignedTicketsAsync(string userId);
        Task<IEnumerable<Ticket>> GetMyTicketsAsync(string userId);
    }
}