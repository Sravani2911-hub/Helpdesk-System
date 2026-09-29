using HelpDesk.DAL.Models;

namespace HelpDesk.Presentation.ViewModels
{
    public class TicketDetailsViewModel
    {
        public Ticket? Ticket { get; set; }

        public IEnumerable<TicketComment> Comments { get; set; } = new List<TicketComment>();

        // NEW
        public IEnumerable<TicketHistory> History { get; set; } = new List<TicketHistory>();
    }
}