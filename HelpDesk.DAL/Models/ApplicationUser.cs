using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? Department { get; set; }

        public string? ProfileImage { get; set; }

        public DateTime MemberSince { get; set; } = DateTime.Now;

        public DateTime? LastLogin { get; set; }

        public ICollection<Ticket>? CreatedTickets { get; set; } //1 can create many tickets ,One User → Many Tickets(One-to-Many Relationship.)

        public ICollection<Ticket>? AssignedTickets { get; set; }
        public ICollection<TicketComment>? TicketComments { get; set; } //One User → Many Comments

        public ICollection<TicketAttachment>? TicketAttachments { get; set; }
        public ICollection<TicketHistory>? TicketHistories { get; set; }
    }
}
