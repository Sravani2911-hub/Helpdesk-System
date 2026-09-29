using HelpDesk.DAL.Models;

namespace HelpDesk.Presentation.Models

{
    public class DashboardViewModel
    {
        public int TotalTickets { get; set; }

        public int OpenTickets { get; set; }

        public int InProgressTickets { get; set; }

        public int ResolvedTickets { get; set; }

        public int ClosedTickets { get; set; }

        // New Dashboard Statistics
        public int TotalEmployees { get; set; }

        public int TotalSupportEngineers { get; set; }

        public int TotalAdmins { get; set; }

        public int CriticalTickets { get; set; }

        public int HighTickets { get; set; }

        public int MediumTickets { get; set; }

        public int LowTickets { get; set; }

        public List<Ticket> RecentTickets { get; set; } = new();

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}