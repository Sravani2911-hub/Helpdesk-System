using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.DAL.Models
{
    public class TicketHistory
    {
        [Key]//this tells EF Core: This is the Primary Key
        public int HistoryId { get; set; }

        [Required] //cannot be empty
        [StringLength(100)]//Maximum length:100 characters
        public string Action { get; set; } = string.Empty;

        [StringLength(50)]
        public string? OldStatus { get; set; } //Open

        [StringLength(50)]
        public string? NewStatus { get; set; } //In progress

        public DateTime ActionDate { get; set; } = DateTime.Now;  //Stores the date and time of the action.

        // Foreign Key - Ticket
        public int TicketId { get; set; } //Links the history record to a ticket.

        [ForeignKey("TicketId")]
        public Ticket? Ticket { get; set; }

        // Foreign Key - User
        public string? UserId { get; set; } //Stores who performed the action.(Emp,Engineer,Teamlead,Admin etc)

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}