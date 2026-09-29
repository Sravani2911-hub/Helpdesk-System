
using HelpDesk.DAL.Models;

namespace HelpDesk.Presentation.Models
{
    public class ReportViewModel
    {
        public int TotalTickets { get; set; }

        public int OpenTickets { get; set; }

        public int InProgressTickets { get; set; }

        public int ResolvedTickets { get; set; }

        public int ClosedTickets { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string? Status { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}