using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.DAL.Models
{
    public class TicketComment
    {
        [Key]//this tells EF Core: This is the Primary Key
        public int CommentId { get; set; }

        [Required]//CommentText cannot be empty
        public string CommentText { get; set; } = string.Empty;

        public DateTime CommentDate { get; set; } = DateTime.Now;

        // Foreign Key - Ticket
        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket? Ticket { get; set; }

        // Foreign Key - User
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}